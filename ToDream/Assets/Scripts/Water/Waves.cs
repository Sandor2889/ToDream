using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [System.Serializable]
    public struct WaveOptions
    {
        public Vector2 _speed;
        public Vector2 _scale;
        public float height;
        public bool _alternate;
    }

    public int _dimensions = 10;
    public WaveOptions[] _waveOptions;
    public float _uvScale;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = gameObject.name;

        _mesh.vertices = GenerateVerts();
        _mesh.triangles = GenerateTries();
        _mesh.uv = GenerateUVs();
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();

        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
    }


    private void Update()
    {
        UpdateVertices();
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_mesh.vertices.Length];

        for(int x = 0; x <= _dimensions; x++)
        {
            for (int z = 0; z <= _dimensions; z++)
            {
                var vec = new Vector2((x / _uvScale) % 2, (z / _uvScale) % 2);
                uvs[Index(x, z)] = new Vector2(
                    vec.x <= 1 ? vec.x : 2 - vec.x,
                    vec.y <= 1 ? vec.y : 2 - vec.y
                    );
            }
        }

        return uvs;
    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(_dimensions + 1) * (_dimensions + 1)];

        for (int x = 0; x <= _dimensions; x++)
        {
            for (int z = 0; z <= _dimensions; z++)
            {
                verts[Index(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int Index(float x, float z)
    {
        return (int)(x * (_dimensions + 1) + z);
    }

    private int[] GenerateTries()
    {
        var tries = new int[_mesh.vertices.Length * 6];

        for (int x = 0; x < _dimensions; x++)
        {
            for (int z = 0; z < _dimensions; z++)
            {
                tries[Index(x, z) * 6 + 0] = Index(x, z);
                tries[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);
                tries[Index(x, z) * 6 + 2] = Index(x + 1, z);
                tries[Index(x, z) * 6 + 3] = Index(x, z);
                tries[Index(x, z) * 6 + 4] = Index(x, z + 1);
                tries[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);
            }
        }
        return tries;
    }

    private void UpdateVertices()
    {
        var verts = _mesh.vertices;
        for (int x = 0; x <= _dimensions; x++)
        {
            for (int z = 0; z <= _dimensions; z++)
            {
                var y = 0f;
                for (int o = 0; o < _waveOptions.Length; o++)
                {
                    if (_waveOptions[o]._alternate)
                    {
                        var perl = Mathf.PerlinNoise(
                            (x * _waveOptions[o]._scale.x) / _dimensions,
                            (z * _waveOptions[o]._scale.y) / _dimensions) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + _waveOptions[o]._speed.magnitude * Time.time) * _waveOptions[o].height;

                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise(
                                (x * _waveOptions[o]._scale.x) / Time.time * _waveOptions[o]._speed.x / _dimensions,
                                (z * _waveOptions[o]._scale.y + Time.time * _waveOptions[o]._speed.y) / _dimensions) - 0.5f;
                        y += perl * _waveOptions[o].height;
                    }
                }
                verts[Index(x, z)] = new Vector3(x, y, z);
            }
        }
        _mesh.vertices = verts;
        _mesh.RecalculateNormals();
    }

    public float GetHeight(Vector3 pos)
    {
        // scale factor and position in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((pos - transform.position), scale);

        // get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        // clamp if the  position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, _dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, _dimensions);
        p2.x = Mathf.Clamp(p1.x, 0, _dimensions);
        p2.z = Mathf.Clamp(p1.z, 0, _dimensions);
        p3.x = Mathf.Clamp(p1.x, 0, _dimensions);
        p3.z = Mathf.Clamp(p1.z, 0, _dimensions);
        p4.x = Mathf.Clamp(p1.x, 0, _dimensions);
        p4.z = Mathf.Clamp(p1.z, 0, _dimensions);

        // get the max distance to one of edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), 
            Vector3.Distance(p2, localPos), 
            Vector3.Distance(p3, localPos), 
            Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
            + (max - Vector3.Distance(p2, localPos))
            + (max - Vector3.Distance(p3, localPos))
            + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        // weighted sum
        var height = _mesh.vertices[Index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
            + _mesh.vertices[Index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
            + _mesh.vertices[Index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
            + _mesh.vertices[Index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        // scale
        return height + transform.lossyScale.y / dist;
    }
}

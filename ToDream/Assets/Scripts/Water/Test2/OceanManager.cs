using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveOptions
    {
        public Vector2 _speed;
        public Vector2 _scale;
        public float height;
        public bool _alternate;
    }

    private readonly int _dimensions = 10;  // 사각형 폴리곤의 수 (plane의 경우 10개이다)
    public MeshFilter _meshFilter;
    private Mesh _mesh;

    public WaveOptions[] _waveOptions;

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
        UpdateWaves();
    }

    private void UpdateWaves()
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
    private int Index(float x, float z)
    {
        return (int)(x * (_dimensions + 1) + z);
    }


    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_mesh.vertices.Length];

        for (int x = 0; x <= _dimensions; x++)
        {
            for (int z = 0; z <= _dimensions; z++)
            {
                //var vec = new Vector2((x / _uvScale) % 2, (z / _uvScale) % 2);
                //uvs[Index(x, z)] = new Vector2(
                //    vec.x <= 1 ? vec.x : 2 - vec.x,
                //    vec.y <= 1 ? vec.y : 2 - vec.y
                //    );
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
}

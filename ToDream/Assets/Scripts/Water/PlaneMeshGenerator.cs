using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMeshGenerator : MonoBehaviour
{
    [Range(2, 256)] public int _resolution = 24;
    [Range(1, 500)] public float _widthX = 10f;
    [Range(1, 500)] public float _widthZ = 10f;
    public Material _material;
    public bool _generateOnPlay = false;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private Vector3[] _verts;
    private int[] _tris;
    private Vector2[] _uvs;

    private void Start()
    {
        if (!_generateOnPlay) return;
        Generate();
    }

    public void Generate()
    {
        Init();
        CreateMeshInfo();
        ApplyMesh();
    }

    private void Init()
    {
        TryGetComponent(out _meshFilter);
        TryGetComponent(out _meshRenderer);

        if (!_meshFilter) { _meshFilter = gameObject.AddComponent<MeshFilter>(); }
        if (!_meshRenderer) {_meshRenderer = gameObject.AddComponent<MeshRenderer>(); }

        _mesh = null;
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        if (_material) { _meshRenderer.material = _material; }
    }

    private void CreateMeshInfo()
    {
        int resolutionZ = (int)(_resolution * _widthZ / _widthX);

        Vector3 widthV3 = new Vector3(_widthX, 0f, _widthZ); // width를 3D로 변환
        Vector3 startPoint = -widthV3 * 0.5f;                // 첫 버텍스의 위치
        Vector2 gridUnit = new Vector2(_widthX / _resolution, _widthZ / resolutionZ); // 그리드 하나의 너비

        Vector2Int vCount = new Vector2Int(_resolution + 1, resolutionZ + 1); // 각각 가로, 세로 버텍스 개수
        int vertsCount = vCount.x * vCount.y;
        int trisCount = _resolution * resolutionZ * 6;

        _verts = new Vector3[vertsCount];
        _tris = new int[trisCount];
        _uvs = new Vector2[vertsCount];

        // 1. 버텍스, UV 초기화
        for (int j = 0; j < vCount.y; j++)
        {
            for (int i = 0; i < vCount.x; i++)
            {
                int index = i + j * vCount.x;
                _verts[index] =
                    startPoint + new Vector3(gridUnit.x * i, 0f, gridUnit.y * j);

                _uvs[index] = new Vector2((float)i / (vCount.x - 1), (float)j / (vCount.y - 1));
            }
        }

        // 2. 폴리곤 초기화
        int tIndex = 0;
        for (int j = 0; j < vCount.y - 1; j++)
        {
            for (int i = 0; i < vCount.x - 1; i++)
            {
                int vIndex = i + j * vCount.x;

                _tris[tIndex + 0] = vIndex;
                _tris[tIndex + 1] = vIndex + vCount.x;
                _tris[tIndex + 2] = vIndex + 1;

                _tris[tIndex + 3] = vIndex + vCount.x;
                _tris[tIndex + 4] = vIndex + vCount.x + 1;
                _tris[tIndex + 5] = vIndex + 1;

                tIndex += 6;
            }
        }
    }

    private void ApplyMesh()
    {
        _mesh.vertices = _verts;
        _mesh.triangles = _tris;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();
    }
}

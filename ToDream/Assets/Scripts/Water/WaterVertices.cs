using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVertices : MonoBehaviour
{ 
    public Mesh _mesh;
    private Vector3[] _vertices;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        _vertices = _mesh.vertices; // water의 정점 배열
        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i].y = WaveManager.GetWaveHeight(transform.position.x + _vertices[i].x);
        }

        _mesh.vertices = _vertices; // 바뀌어진 값을 다시 넣어줌
        _mesh.RecalculateNormals(); // 변경된 Vertices들에 대해 법선 재연산
    }
}

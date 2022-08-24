using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private Image _playerIcon;
    [SerializeField] private Image _questTargetIcon;
    [SerializeField] private Image _marking;

    public void Start()
    {
        InitICon();
        //GridMap _map = new GridMap(10, 10, 100);
    }


    private void InitICon()
    {
        _playerIcon.sprite = Resources.Load<Sprite>("MapMarker/PlayerIcon");
    }

    private IEnumerator UpdateMyLocation()
    {
        while (true)
        {

            yield return null;
        }
    }
}

public class GridMap
{
    private int _width;
    private int _height;
    private int _cellSize;

    private Vector3[,] _gridMap;

    public GridMap(int width, int height, int cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridMap = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _gridMap[x, z] = new Vector3(x, 0, z);
                var pos = GetWorldPos(x, z);

                Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x + 1, z), Color.blue);
                Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x, z + 1), Color.blue);
            }
        }
    }

    private Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * _cellSize;
    }
}

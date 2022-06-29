using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public PlaneMeshGenerator[] _planes;
    public int _resolution = 24;
    public int _widthX = 10;
    public int _widthY = 10;

    private void Awake()
    {
        _planes = GetComponentsInChildren<PlaneMeshGenerator>();

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _widthX; i++)
        {
            for (int j = 0; j < _widthY; j++)
            {
                //_planes[i]
            }
        }
    }
}

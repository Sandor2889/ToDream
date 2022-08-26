using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTr;
    [SerializeField] private GameObject _mapPing;
    private float _rectSizeX;
    private float _rectSizeY;
    [SerializeField] private Vector2 _offset = new Vector2(485, 65);

    private void Awake()
    {
        _rectSizeX = _rectTr.rect.size.x;
        _rectSizeY = _rectTr.rect.size.y;    
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Vector3 worldMapPos;
            worldMapPos.x = GetClickedPosRatio().x * _rectSizeX;
            worldMapPos.y = 0;
            worldMapPos.z = GetClickedPosRatio().y * _rectSizeY;

            OnMapPing(worldMapPos);
        }
    }

    private void OnMapPing(Vector3 pos)
    {
        _mapPing.transform.position = pos;
        _mapPing.SetActive(true);
    }

    private Vector2 GetClickedPosRatio()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 clickedPos = mousePos - _offset;
        float ratioVecX = (clickedPos.x / _rectSizeX);
        float ratioVecY = (clickedPos.y / _rectSizeY);
        Vector2 ratioVec = new Vector2(ratioVecX - 0.5f, ratioVecY - 0.5f);
        return ratioVec;
    }

    public void OpenMap()
    {
        gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        gameObject.SetActive(false);
    }

}
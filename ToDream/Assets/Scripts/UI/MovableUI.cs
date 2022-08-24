using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Transform _targetTr; // 이동될 UI

    private Vector2 _startingPos;
    private Vector2 _moveBegin;
    private Vector2 _Offset;

    private void Awake()
    {
        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (_targetTr == null) { _targetTr = transform.parent; }
    }

    // 드래그 시작 위치 지정
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _startingPos = _targetTr.position;
        _moveBegin = eventData.position;
    }


    // 드래그 : 마우스 커서 위치로 이동
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _Offset = eventData.position - _moveBegin;
        _targetTr.position = _startingPos + _Offset;
    }
}

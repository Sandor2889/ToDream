using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Transform _targetTr; // �̵��� UI

    private Vector2 _startingPos;
    private Vector2 _moveBegin;
    private Vector2 _Offset;

    private void Awake()
    {
        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �θ�� �ʱ�ȭ
        if (_targetTr == null) { _targetTr = transform.parent; }
    }

    // �巡�� ���� ��ġ ����
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _startingPos = _targetTr.position;
        _moveBegin = eventData.position;
    }


    // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _Offset = eventData.position - _moveBegin;
        _targetTr.position = _startingPos + _Offset;
    }
}

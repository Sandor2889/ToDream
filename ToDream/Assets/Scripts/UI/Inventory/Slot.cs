using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private VehicleRegistration _registration;
    [SerializeField] private Vector3 _offset;
    
    public Item _item;
    public Image _itemImage;

    public void Awake()
    {
        _registration = FindObjectOfType<VehicleRegistration>(true);
    }

    public void SetItem(Item item)
    {
        _item = item;
        _itemImage.sprite = item._sprite;
        SetAlpha(1);
    }

    public void Clear()
    {
        _itemImage.sprite = null;
        _item = null;
        SetAlpha(0);
    }

    public void SetAlpha(int alpha)
    {
        Color color = _itemImage.color;
        color.a = alpha;
        _itemImage.color = color;
    }

    // 슬롯 클릭시 탈것 등록 버튼 호출
    public void OnRegistrationButton()
    {
        if (_item == null) { return; }

        _registration.gameObject.SetActive(true);
        _registration._item = _item;
        _registration._buttonPanel.transform.position = gameObject.transform.position + _offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlot : MonoBehaviour
{
    [SerializeField] private int _boostCost;
    public Item _item;
    public Image _image;
    public Text _text;
    public int _count;

    public void UpdateItem(Item item, int itemCount)
    {
        if (_item != null && _count <= 0) 
        {
            UIManager._Instance._InventoryUI.UpdateGold(-_boostCost);
            return; 
        } 

        if (_item == null)  // 소모품을 첫 획득시 (0개에서 1개 될때)
        {
            _item = item;
            _image.sprite = item._sprite;
            _count = 1;
            SetAlpha(1);
        }
        else
        {
            _count += itemCount;

            if (_count <= 0) { Clear(); }
        }
        _text.text = "x" + _count.ToString();
    }

    public void Clear()
    {
        _image.sprite = null;
        SetAlpha(0);
    }

    public void SetAlpha(int alpha)
    {
        Color color = _image.color;
        color.a = alpha;
        _image.color = color;
    }
}

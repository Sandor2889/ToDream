using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlot : MonoBehaviour
{
    public Item _item;
    public Image _image;
    public Text _text;

    public void UpdateItem(Item item, int itemCount)
    {
        //if (_item != null && _item._count <= 0) { return; }  // 필요시 아이템 부족 메세지 띄우기

        //if (_item == null)  // 소모품을 첫 획득시 (0개에서 1개 될때)
        //{
        //    _item = item;
        //    _image.sprite = item._itemSprite;
        //    _item._count = 1;
        //    SetAlpha(1);
        //}
        //else
        //{
        //    _item._count += itemCount;

        //    if (_item._count <= 0) { Clear(); }
        //}
        //_text.text = "x" + item._count.ToString();
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

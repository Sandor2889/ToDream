using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image _itemImage;
    public Item _item;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //public Item item; // 획득한 아이템
    public Image itemImage; // 아이템 이미지

    // 아이템 획득 여부에따라 슬롯 이미지 알파값 조정
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(/*Item _item*/)
    {
        SetColor(1);
        //item = _item;
        //itemImage.sprite = _item.itemImage;
    }

    // 아이템 제거
    public void ClearItem()
    {
        //item = null;
        //itemImage.sprite = null;
        SetColor(0);
    }
}

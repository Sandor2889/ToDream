using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public Image itemImage; // 아이템 이미지

    // 아이템 획득 여부에따라 슬롯 이미지 알파값 조정
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item item)
    {
        SetColor(1);
        this.item = item;
        itemImage.sprite = item.itemImage;
    }

    // 아이템 제거
    public void ClearItem()
    {
        item = null;
        itemImage.sprite = null;
        SetColor(0);
    }
}

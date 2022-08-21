using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "Item_")]
public class Item : ScriptableObject
{
    public Category _category;      // 아이템 카테고리
    public GameObject _obj;         // 아이템 프리펩
    public Sprite _sprite;          // 아이템 Sprite
    public int _key;                // 아이템 key
}

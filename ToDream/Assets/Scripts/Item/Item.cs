using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "Item_")]
public class Item : ScriptableObject
{
    public Category _category;      // ������ ī�װ�
    public GameObject _obj;         // ������ ������
    public Sprite _sprite;          // ������ Sprite
    public int _key;                // ������ key
}

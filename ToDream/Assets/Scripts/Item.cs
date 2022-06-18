using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Vehicle,
    Consumable
}

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item ")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public string ItemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
}

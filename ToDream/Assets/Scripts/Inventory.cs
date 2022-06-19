using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private Slot[] bag;
    private bool inActive;

    private void Awake()
    {
        bag = inventory.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // 인벤토리 On/OFF
    private void ToggleInventory()
    {
        inActive = !inActive;
        inventory.SetActive(inActive);
    }

    // 아이템 습득
    public void AcquireItem(Item item)
    {
        for (int idx = 0; idx < bag.Length; idx++)
        {
            if (bag[idx].item == null)
            {
                bag[idx].AddItem(item);
                return;
            }
        }
    }
}

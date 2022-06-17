using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] bag;

    private void Awake()
    {
        bag = GetComponentsInChildren<Slot>();
    }
}

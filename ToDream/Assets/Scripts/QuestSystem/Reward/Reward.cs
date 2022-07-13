using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] string _description;
    [SerializeField] int _quantity;

    public Sprite _Icon => _icon;
    public string _Description => _description;
    public int Quantity => _quantity;

    public abstract void Give(Quest quest);
}

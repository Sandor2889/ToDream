using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Text _title;
    public Text _description;
    //public Reward _reward;

    public void SetText(string title, string desc)
    {
        _title.text = title;
        _description.text = desc;
    }
}

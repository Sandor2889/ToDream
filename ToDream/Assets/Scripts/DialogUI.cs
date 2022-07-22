using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private Text _dialog;

    public void SetText(string msg)
    {
        _dialog.text = msg;
    }
}

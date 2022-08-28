using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldMapUI : MonoBehaviour
{
    [SerializeField] private GameObject _worldCam;

    public void OpenMap()
    {
        UIManager.CursorVisible(true);
        UIManager._isOpendUI = true;
        gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        UIManager.CursorVisible(false);
        UIManager._isOpendUI = false;
        gameObject.SetActive(false);
    }
}
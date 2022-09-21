using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            UIManager._Instance._WorldMapUI.RemovePing();
            gameObject.SetActive(false);
        }
    }
}

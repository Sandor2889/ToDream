using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    public string key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager._Instance.ReceiveReport(key, 1);
        }
    }
}

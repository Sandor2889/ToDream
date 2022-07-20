using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTestArea : MonoBehaviour
{
    public string questKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager._Instance.ReceiveReport(questKey, 1);
        }
    }
}

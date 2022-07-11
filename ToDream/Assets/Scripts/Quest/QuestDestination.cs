using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        QuestManager._Instance.AddQuestItem("Test", 1);   
    }
}

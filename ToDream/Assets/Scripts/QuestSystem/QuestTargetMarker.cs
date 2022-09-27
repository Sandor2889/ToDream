using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    public string _key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quest의 목표 key 전달
            QuestManager._Instance.ReceiveReport(_key, 1);
        }
    }

    // 퀘스트를 수락하면 퀘스트 마커 활성화
    public void OnQuestMarker()
    {
        gameObject.SetActive(true);
    }

    public void OffQuestMarker()
    {
        gameObject.SetActive(false);
    }
}

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
            // Quest�� ��ǥ key ����
            QuestManager._Instance.ReceiveReport(_key, 1);
        }
    }

    // ����Ʈ�� �����ϸ� ����Ʈ ��Ŀ Ȱ��ȭ
    public void OnQuestMarker()
    {
        gameObject.SetActive(true);
    }

    public void OffQuestMarker()
    {
        gameObject.SetActive(false);
    }
}

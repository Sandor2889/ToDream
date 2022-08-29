using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMarkerUI : MonoBehaviour
{
    [SerializeField] private QuestGiver[] _npcs;


    // (int ����Ʈ������, QuestState ����Ʈ �������)
    public void SettingByQuestState(int idx, QuestState questState)
    {
        OnMarker(idx, questState);
    }


    public void OnMarker(int idx, QuestState state)
    {
        QuestGiver giver = _npcs[idx];
        int intState = (int)state + 1;  // 0�� ���� �Ұ��� �����̹Ƿ� 1���� ����

        for(int i = 0; i < giver._Markers.Length; i++)
        {
            // ���� �Ұ� ���³� �Ϸ�� ���¶�� ��� ��Ŀ ����
            if (state == QuestState.Unvaliable || state == QuestState.Done)
            {
                giver._Markers[i].gameObject.SetActive(false);
                continue;
            }

            if (intState == i)
            {
                giver._Markers[i].gameObject.SetActive(true);
            }
            else 
            {
                giver._Markers[i].gameObject.SetActive(false);
            }
        }
    }
}

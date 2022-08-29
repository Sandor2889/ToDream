using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMarkerUI : MonoBehaviour
{
    [SerializeField] private QuestGiver[] _npcs;


    // (int 퀘스트제공자, QuestState 퀘스트 진행상태)
    public void SettingByQuestState(int idx, QuestState questState)
    {
        OnMarker(idx, questState);
    }


    public void OnMarker(int idx, QuestState state)
    {
        QuestGiver giver = _npcs[idx];
        int intState = (int)state + 1;  // 0은 수락 불가능 상태이므로 1부터 시작

        for(int i = 0; i < giver._Markers.Length; i++)
        {
            // 수락 불가 상태나 완료된 상태라면 모든 마커 종료
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

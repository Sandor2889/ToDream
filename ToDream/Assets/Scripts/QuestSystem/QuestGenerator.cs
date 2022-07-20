using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트의 생성, 등록, 취소역할
/// 등록시 QuestManager._quests에 추가
/// </summary>
public class QuestGenerator : MonoBehaviour
{
    public Quest _quest;

    [HideInInspector] public bool _enable = false;

    public void CreateQuest()
    {
        _quest = new Quest();
        _enable = true;
    }

    public void RegisterQuest()
    {
        QuestManager._Instance._quests.Add(_quest);

        Cancel();
    }

    public void Cancel()
    {
        _quest = null;
        _enable = false;
    }
}

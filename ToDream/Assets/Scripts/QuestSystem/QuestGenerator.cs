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

    public int _talkBoxIdx;           // talk 지정 Idx (TalkList.RemoveAt)
    public bool _enable;              // GUI - FoldOut
    public int _insertIdx;            // Quest 지정 Idx (QuestList.Insert)
    public int _previousIdx;          // Edit전 Idx 위치
    public string _insertStr;         // Edit전 Idx 위치 string 표기


    #region Main Button
    public void CreateQuest()
    {
        _quest = new Quest();
        _insertIdx = QuestManager._Instance._quests.Count;
        _insertStr = "[ This is new one ] ";
        AddQuestGoal();
        _enable = true;
    }

    public void RegisterQuest()
    {
        if (_insertIdx > QuestManager._Instance._quests.Count)
        {
            Debug.LogWarning("Insert idx set from " + _insertIdx + " to " + QuestManager._Instance._quests.Count);
            _insertIdx = QuestManager._Instance._quests.Count;
        }

        QuestManager._Instance._quests.Insert(_insertIdx, _quest);
        Cancel();
    }

    public void Cancel()
    {
        _talkBoxIdx = 0;
        _insertIdx = QuestManager._Instance._quests.Count;
        _quest = null;
        _enable = false;
    }
    #endregion

    #region Talk Button
    public void AddTalk()
    {
        _quest._talk.Add("");
    }

    public void RemoveAtTalk()
    {
        if (_talkBoxIdx >= _quest._talk.Count)
        {
            Debug.LogWarning("QuestGenerator.cs -> Index out of List");
            return;
        }
        
        _quest._talk.RemoveAt(_talkBoxIdx);
    }
    #endregion

    #region QuestGoal Button
    public void AddQuestGoal()
    {
        QuestGoal questGoal = new QuestGoal();
        _quest._questGoals.Add(questGoal);
    }

    public void RemoveQuestGoal()
    {
        int last = _quest._questGoals.Count - 1;

        // QuestGoal이 단 하나 남아있을때 '-'를 시도하였을경우
        if (last == 0) 
        {
            Debug.LogWarning("You can`t remove last one of QuestGoal");
            return;
        }
        _quest._questGoals.RemoveAt(last);
    }
    #endregion

    // Edit : 다시 편집기로 이동
    public void Edit(Quest quest)
    {
        _quest = quest;
        _previousIdx = QuestManager._Instance._quests.IndexOf(quest);
        QuestManager._Instance._quests.Remove(quest);
        _insertIdx = QuestManager._Instance._quests.Count;
        _insertStr = "[ Previous Idx " + _previousIdx + " ]";
        _enable = true;

    }


}

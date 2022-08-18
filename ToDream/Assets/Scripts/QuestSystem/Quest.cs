using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum QuestState
{
    Unvaliable,     // 수락할 수 없는 퀘스트 (선행퀘)
    Avaliable,      // 수락 가능한 퀘스트
    Accepted,       // 수락한 퀘스트
    Completed,      // 목표 달성한 퀘스트
    Done            // 완료한 퀘스트
}

[System.Serializable]
public struct Reward
{
    public int _gold;
    public Item _item; 
}

// private 필드변수에 serialize 속성을 붙이지 않으면
// 실행시 커스텀에디터로 수정한 값이 초기화 됨.
[System.Serializable]
public class Quest
{
    public QuestState _questState;                                    // 퀘스트 상태
    public NPCName _npcName;                                          // 퀘스트 제공자
    public string _title;                                             // 퀘스트 제목
    public int _questCode;                                            // 퀘스트 고유번호
    public string _description;                                       // 퀘스트 설명
    public List<QuestGoal> _questGoals = new List<QuestGoal>();       // 퀘스트 목표 그룹
    public Reward _reward;                                            // 퀘스트 완료 보상

    // Dialog
    public List<string> _talk = new List<string>();                   // NPC 대화
    public int _openQuestIdx;                                         // 해당 talk idx에서 다음 대화로 넘어갈시 퀘스트 창 오픈

    // Option
    public List<ConditionPrecedent> _conditions = new List<ConditionPrecedent>();    // 선행 퀘스트 조건
    public bool _hasConditions;                                        // 선행 퀘스트 유무
    public bool _autoComplete;                                         // 퀘스트 자동 완료
    public bool _detailFolded;                                         // GUI 상세설명 접기
    public bool _talkFolded;                                           // GUI npc 대화상자 접기

    // Event
    public System.Action _nextQuest;                                   // QuestGiver의 현재 퀘스트 완료시 다음 퀘스트로 셋팅
    public System.Action<int, QuestState> _onNPCMarker;                // QuestGiver의 현재 퀘스트 상태에 따른 Marker 업데이트

    // 모든 GoalState가 Complete라면 true 반환
    public bool _IsAllGoalCompleted => _questGoals.All(x => x._goalState == GoalState.Complete);

    // 모든 선행 퀘스트가 완료 되면 true 반환
    public bool _IsAllQuestCompleted => _conditions.All(x => x._isCompleted);

    // 퀘스트 달성률 업데이트
    public void ReceiveReport(string target, int counting)
    {
        foreach (var goal in _questGoals)
        {
            if (!IsTarget(goal, target)) { continue; }
            goal._currentTargetCount += counting;

            // 목표량 달성시 QuestGoal의 State 완료 상태로 전환
            if (goal._currentTargetCount >= goal._requiredAmount)
            {
                goal.Complete();
            }
        }

        if (_IsAllGoalCompleted)
        {
            Complete();

            if(_autoComplete)
            {
                Done();
            }
            UIManager._Instance._QuestListUI.UpdateList(_questCode);
        }
    }

    // 퀘스트의 Target 조건 확인
    public bool IsTarget(QuestGoal goal, string target)
    {
        int hashGoal = goal._target.GetHashCode();
        int hashTarget = target.GetHashCode();

        if (hashTarget == hashGoal && goal._goalState == GoalState.InProgress)
        {
            return true;
        }

        return false;
    }

    // 퀘스트 고유번호 초기화
    public void InitCode()
    {
        _questCode += (int)_npcName * 100;
    }

    // 선행 조건 확인하여 퀘스트 수락 가능 상태로 전환
    public void CompleteAllCondition( )
    {
        for(int i = 0; i < _conditions.Count; i++)
        {
            if (!_conditions[i]._isCompleted) { return; }
        }

        Avaliable();
    }

    // 퀘스트 완료 보상 획득
    public void GiveReward()
    {
        UIManager._Instance._InventoryUI._Gold = _reward._gold;
        if (_reward._item != null)
        {
            UIManager._Instance._InventoryUI.AcquireItem(_reward._item);
        }
    }

    public void UploadMyCode(int code)
    {
        QuestManager._Instance.FindQuestAsCode(code);
    }

    #region 퀘스트 상태 변환
    public void Avaliable()
    {
        _questState = QuestState.Avaliable;

        _onNPCMarker?.Invoke((int)_npcName, _questState);
    }

    public void Accepted()
    {
        _questState = QuestState.Accepted;
        _onNPCMarker?.Invoke((int)_npcName, _questState);
        foreach (var goal in _questGoals)
        {
            goal.OnQuestMarker();
        }
    }

    public void Complete()
    {
        _questState = QuestState.Completed;
        _onNPCMarker?.Invoke((int)_npcName, _questState);
        Debug.Log("The " + _title + " is completed");
    }

    public void Done()
    {
        _questState = QuestState.Done;

        _onNPCMarker?.Invoke((int)_npcName, _questState);
        _onNPCMarker = null;
        _nextQuest?.Invoke();
        _nextQuest = null;        

        QuestManager._Instance._acceptedQuests.Remove(this);
        QuestManager._Instance._doneQuests.Add(this);

        GiveReward();
        UploadMyCode(_questCode);
    }
    #endregion
}


// 선행 조건을 가질경우 보유
[System.Serializable]
public class ConditionPrecedent
{
    public int _code;                   // 선행 퀘스트의 고유번호
    public bool _isCompleted;           // 완료 유무

    public void UpdateConditions(int code)
    {
        if (_code == code)
        {
            _isCompleted = true;
        }
    }
}
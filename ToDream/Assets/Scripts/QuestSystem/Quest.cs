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
    //public Item _item; 
}

// private 필드변수에 serialize 속성을 붙이지 않으면
// 플레이시 에디터로 수정한 값이 초기화 됨.
[System.Serializable]
public class Quest
{
    public QuestState _questState;                                    // 퀘스트 상태
    public NPCName _npcName;                                          // 퀘스트 제공자
    public string _title;                                             // 퀘스트 제목
    public string _description;                                       // 퀘스트 설명
    public List<QuestGoal> _questGoals = new List<QuestGoal>();       // 퀘스트 목표 그룹
    public Reward _reward;                                            // 퀘스트 완료 보상

    public List<string> _talk = new List<string>();                   // NPC 대화
    public int _openQuestIdx;                                         // 해당 talk idx에서 다음 대화로 넘어갈시 퀘스트 창 오픈

    public bool _autoComplete;                                        // 퀘스트 자동 완료
    public bool _detailFolded;                                        // GUI 상세설명 접기
    public bool _talkFolded;                                          // GUI npc 대화상자 접기

    public System.Action _nextQuest;                                  // QuestGiver의 현재 퀘스트 완료시 다음 퀘스트로 셋팅

    // 모든 Goal의 State가 Complete라면 true 반환
    public bool _AllComplete => _questGoals.All(x => x._goalState == GoalState.Complete);

    // 퀘스트 달성률 업데이트
    public void ReceiveReport(QuestTarget target, int counting)
    {
        foreach (var goal in _questGoals)
        {
            if (!IsTarget(goal, target)) { continue; }
            goal._currentTargetCount += counting;
            Debug.Log(goal._subTitle + " : " + goal._currentTargetCount + " / " + goal._requireAmount);

            // 목표량 달성시 QuestGoal의 State 완료 상태로 전환
            if (goal._currentTargetCount >= goal._requireAmount)
            {
                goal.Complete();
            }
        }

        if (_AllComplete)
        {
            Debug.Log("All complete");
            Complete();

            if(_autoComplete)
            {
                Done();
            }
        }
    }

    // 퀘스트의 Target과 State 조건 확인
    public bool IsTarget(QuestGoal goal, QuestTarget target)
    {
        if (target == goal._target && goal._goalState == GoalState.InProgress)
        {
            return true;
        }

        return false;
    }

    public void GiveReward()
    {
        Debug.Log("Get Reward!!!");
    }

    #region 퀘스트 상태 변환
    public void Avaliable()
    {
        _questState = QuestState.Avaliable;
    }

    public void Accepted()
    {
        _questState = QuestState.Accepted;
        foreach (var goal in _questGoals)
        {
            goal.OnQuestMarker();
        }
    }

    public void Complete()
    {
        _questState = QuestState.Completed;
        Debug.Log("The " + _title + " is completed");
    }

    public void Done()
    {
        _questState = QuestState.Done;
        _nextQuest();
        _nextQuest = null;
        GiveReward();
        QuestManager._Instance._acceptedQuests.Remove(this);
        QuestManager._Instance._doneQuests.Add(this);
    }

    public void Cancel()
    {
        _questState = QuestState.Avaliable;
    }
    #endregion
}

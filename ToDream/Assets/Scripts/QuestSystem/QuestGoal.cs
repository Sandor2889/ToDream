using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GoalState
{
    InProgress,     // 진행중
    Complete        // 목표량 달성
}

[System.Serializable]
public class QuestGoal
{
    public GoalState _goalState;                          // 진행 상태
    public string _target;                                // 퀘스트 목표
    public string _subTitle;                              // 서브 타이틀
    public int _currentTargetCount;                       // 현재 수량
    public int _requiredAmount;                           // 요구 수량
    public QuestTargetMarker _targetMarker;               // 퀘스트 목표 지점 마커 활성화
    public bool _isFolded;                                // GUI 접기 기능


    // GoalState 전환 - 완료 
    public void Complete()
    {
        _goalState = GoalState.Complete;
        OffQuestMarker();
    }

    // 퀘스트 취소시
    public void Cancel()
    {
        _goalState = GoalState.InProgress;
        OffQuestMarker();
        _currentTargetCount = 0;
    }

    // 퀘스트를 수락하면 퀘스트 마커 활성화
    public void OnQuestMarker()
    {
        _targetMarker.gameObject.SetActive(true);
    }

    public void OffQuestMarker()
    {
        _targetMarker.gameObject.SetActive(false);
    }
}

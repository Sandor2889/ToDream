using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum QuestState
{
    Unvaliable,     // 수락할 수 없는 퀘스트 (선행퀘)
    Avaliable,      // 수락 가능한 퀘스트
    Accepted,       // 수락한 퀘스트
    Completed,      // 목표 달성한 퀘스트
    Done            // 완료한 퀘스트
}


// private 필드변수에 serialize 속성을 붙이지 않으면
// 플레이시 에디터로 수정한 값이 초기화 됨.
[System.Serializable]
public class Quest
{
    public QuestState _questState;                        // 퀘스트 상태
    public NPCName _npcName;                              // 퀘스트 제공자

    public string _title;                                 // 퀘스트 제목
    public string _description;                           // 퀘스트 설명

    public string _target;                                // 퀘스트 목표
    public int _currentTargetCount;                       // 현재 수량
    public int _requireAmount;                            // 요구 수량
    public QuestTestArea _targetMarker;                   // 퀘스트 목표 지점 마커 활성화  

    public List<string> _talk = new List<string>();       // NPC 대화
    public int _openQuestIdx;                           // 퀘스트 수락 후 작별 인사

    public Reward _reward;                                                 // 보상

    public bool _autoComplete;                                             // 퀘스트 자동 완료

    public bool _detailFolded;                                                 // GUI 상세설명 접기
    public bool _talkFolded;                                                   // GUI npc 대화상자 접기

    public System.Action _nextQuest;

    // 퀘스트 달성률 업데이트
    public void ReceiveReport(string target, int counting)
    {
        if (!IsTarget(target)) { return; }

        _currentTargetCount += counting;
        Debug.Log(_title + " : " + _currentTargetCount + " / " + _requireAmount);

        // 목표랑 달성시 퀘스트 완료 상태로 전환
        if (_currentTargetCount >= _requireAmount) 
        { 
            Complete(); 

            // Auto 체크시 자동 완료
            if (_autoComplete)
            {
                // 완료처리
                Done();
            }
        }
    }

    // 퀘스트의 타겟이 맞는지 비교
    public bool IsTarget(string target)
    {
        if (_target == target && _questState == QuestState.Accepted)
        {
            return true;
        }

        return false;
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

    #region 퀘스트 상태 변환
    public void Avaliable()
    {
        _questState = QuestState.Avaliable;
    }

    public void Accepted()
    {
        _questState = QuestState.Accepted;
        OnQuestMarker();
    }

    public void Complete()
    {
        _questState = QuestState.Completed;
        OffQuestMarker();
        Debug.Log("The " + _title + " is completed");
    }

    public void Done()
    {
        _questState = QuestState.Done;
        _nextQuest();
        QuestManager._Instance._acceptedQuests.Remove(this);
        QuestManager._Instance._doneQuests.Add(this);
    }

    public void Cancel()
    {
        _questState = QuestState.Avaliable;
    }
    #endregion
}

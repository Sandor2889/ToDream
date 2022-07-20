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
    //[SerializeField] int _questID;                                  // 퀘스트 고유번호
    [SerializeField] QuestState _questState;                        // 퀘스트 상태

    [SerializeField] string _npcName;                               // 퀘스트 제공자
    [SerializeField] string _title;                                 // 퀘스트 제목
    [SerializeField] string _description;                           // 퀘스트 설명
    // List<string> _talk = new List<string>();       // NPC 대화

    [SerializeField] string _target;                                // 퀘스트 목표
    [SerializeField] int _currentTargetCount;                       // 현재 수량
    [SerializeField] int _requireAmount;                            // 요구 수량
    [SerializeField] QuestTestArea _targetMarker;                   // 퀘스트 목표 지점 마커 활성화  

    Reward _reward;                                                 // 보상

    bool _isFolded;                                                 // GUI 접기 기능



    //public int _QuestID { get { return _questID; } set { _questID = value; } }
    public QuestState _QuestState { get { return _questState; } set { _questState = value; } }

    public string _NpcName { get { return _npcName; } set { _npcName = value; } }

    public string _Title { get { return _title; } set { _title = value; } }

    public string _Description { get { return _description; } set { _description = value; } }

    public string _Target { get { return _target; } set { _target = value; }  }

    public int _CurrentTargetCount { get { return _currentTargetCount; } set { _currentTargetCount = value; } }

    public int _RequireAmount { get { return _requireAmount; } set { _requireAmount = value; } }

    public QuestTestArea _TargetMarker { get { return _targetMarker; } set { _targetMarker = value; } }

    public bool _IsFolded { get { return _isFolded; } set { _isFolded = value; } }

    // 퀘스트 달성률 업데이트
    public void ReceiveReport(string target, int counting)
    {
        if (!IsTarget(target)) { return; }

        _currentTargetCount += counting;
        Debug.Log(_title + " : " + _currentTargetCount + " / " + _requireAmount);

        // 목표랑 달성시 퀘스트 완료 상태로 전환
        if (_currentTargetCount >= _requireAmount) { Complete(); }
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
        Debug.Log("This quest is completed");
    }

    public void Done()
    {
        _questState = QuestState.Done;
    }

    public void Cancel()
    {
        _questState = QuestState.Avaliable;
    }
    #endregion
}

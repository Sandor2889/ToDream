using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NPC 이름
public enum NPCName
{
    Mother,
    Manager,
    President
}

[DefaultExecutionOrder(201)]
public class QuestGiver : MonoBehaviour
{
    private int _currentQuestIdx;

    // Npc 퀘스트 진행 상태 마커 (파티클)
    [SerializeField] private ParticleSystem[] _markers;

    public ParticleSystem[] _Markers => _markers;

    public List<Quest> _myQuests = new List<Quest>();
    public NPCName _npcName;
    public Image _myImage;  // WorldMap 연동

    public Quest _CurrentQuest
    {
        get
        {
            // 더이상 퀘스트가 없을때
            if (_currentQuestIdx >= _myQuests.Count)
            {
                return null;
            }
                       
            return _myQuests[_currentQuestIdx];
        }
    }

    private void Awake()
    {
        _markers = GetComponentsInChildren<ParticleSystem>(true);
        DistributeQuests();
        UpdateCurrentState();

        if (_CurrentQuest == null ) { return; }
        _CurrentQuest._onNPCMarker?.Invoke((int)_npcName, _CurrentQuest._questState);
    }

    // 각 NPC의 Quest 가져오기
    private void DistributeQuests()
    {
        QuestManager questMgr = QuestManager._Instance;
        _myQuests = questMgr._questContainer.FindAll(x => x._npcName.GetHashCode() == _npcName.GetHashCode());

        if (_myQuests.Count <= 0) { return; }

        // 각 Quest에 이벤트 등록
        foreach (var quest in _myQuests)
        {
            quest._nextQuest += NextQuestIdx;
            quest._onNPCMarker += UIManager._Instance._NPCMarkerUI.SettingByQuestState;
        }
    }

    public void UpdateCurrentState()
    {
        // 현재 퀘스트가 선행 조건 체크가 없으면 Avaliable로 전환
        if (_CurrentQuest != null && 
            !_CurrentQuest._hasConditions && 
            _CurrentQuest._questState == QuestState.Unvaliable)
        {
            _myQuests[_currentQuestIdx].Avaliable();
        }
    }

    private void NextQuestIdx()
    {
        _currentQuestIdx++;
        UpdateCurrentState();
    }
}

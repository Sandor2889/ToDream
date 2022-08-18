using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC 이름
public enum NPCName
{
    Default,
    Condition
}

[DefaultExecutionOrder(201)]
public class QuestGiver : MonoBehaviour
{
    private int _currentQuestIdx;

    public List<Quest> _quests = new List<Quest>();
    public NPCName _npcName;

    public Quest _CurrentQuest
    {
        get
        {
            // 더이상 퀘스트가 없을때
            if (_currentQuestIdx >= _quests.Count)
            {
                return null;
            }
                       
            return _quests[_currentQuestIdx];
        }
    }

    private void Awake()
    {
        DistributeQuests();
        UpdateCurrentState();

        if (_CurrentQuest == null ) { return; }
        _CurrentQuest._onNPCMarker?.Invoke((int)_npcName, _CurrentQuest._questState);
    }

    // 각 NPC의 Quest 가져오기
    private void DistributeQuests()
    {
        QuestManager questMgr = QuestManager._Instance;
        _quests = questMgr._quests.FindAll(x => x._npcName.GetHashCode() == _npcName.GetHashCode());

        if (_quests.Count >= 0) { return; }

        foreach (var quest in _quests)
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
            _quests[_currentQuestIdx].Avaliable();
        }
    }

    private void NextQuestIdx()
    {
        _currentQuestIdx++;
        UpdateCurrentState();
    }
}

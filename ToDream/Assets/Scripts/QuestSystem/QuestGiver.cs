using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NPC 이름
public enum NPCName
{
    Default
}

public class QuestGiver : MonoBehaviour
{
    private int _currentQuestIdx;

    public List<Quest> _quests = new List<Quest>();
    public NPCName _npcName;
    private bool _hasQuest;                              // NPC 머리위에 표시하기 위한 유무 체크 

    public Quest _CurrentQuest
    {
        get
        {
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
    }

    private void DistributeQuests()
    {
        QuestManager questMgr = QuestManager._Instance;
        _quests = questMgr._quests.FindAll(x => x._npcName == _npcName);

        foreach (var quest in _quests)
        {
            quest._nextQuest += NextQuestIdx;
        }
    }

    private void NextQuestIdx()
    {
        _currentQuestIdx++;
    }
}

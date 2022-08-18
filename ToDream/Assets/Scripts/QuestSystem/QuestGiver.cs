using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC �̸�
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
            // ���̻� ����Ʈ�� ������
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

    // �� NPC�� Quest ��������
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
        // ���� ����Ʈ�� ���� ���� üũ�� ������ Avaliable�� ��ȯ
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

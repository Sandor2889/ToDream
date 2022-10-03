using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// NPC �̸�
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

    // Npc ����Ʈ ���� ���� ��Ŀ (��ƼŬ)
    [SerializeField] private ParticleSystem[] _markers;

    public ParticleSystem[] _Markers => _markers;

    public List<Quest> _myQuests = new List<Quest>();
    public NPCName _npcName;
    public Image _myImage;  // WorldMap ����

    public Quest _CurrentQuest
    {
        get
        {
            // ���̻� ����Ʈ�� ������
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

    // �� NPC�� Quest ��������
    private void DistributeQuests()
    {
        QuestManager questMgr = QuestManager._Instance;
        _myQuests = questMgr._questContainer.FindAll(x => x._npcName.GetHashCode() == _npcName.GetHashCode());

        if (_myQuests.Count <= 0) { return; }

        // �� Quest�� �̺�Ʈ ���
        foreach (var quest in _myQuests)
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
            _myQuests[_currentQuestIdx].Avaliable();
        }
    }

    private void NextQuestIdx()
    {
        _currentQuestIdx++;
        UpdateCurrentState();
    }
}

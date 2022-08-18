using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum QuestState
{
    Unvaliable,     // ������ �� ���� ����Ʈ (������)
    Avaliable,      // ���� ������ ����Ʈ
    Accepted,       // ������ ����Ʈ
    Completed,      // ��ǥ �޼��� ����Ʈ
    Done            // �Ϸ��� ����Ʈ
}

[System.Serializable]
public struct Reward
{
    public int _gold;
    public Item _item; 
}

// private �ʵ庯���� serialize �Ӽ��� ������ ������
// ����� Ŀ���ҿ����ͷ� ������ ���� �ʱ�ȭ ��.
[System.Serializable]
public class Quest
{
    public QuestState _questState;                                    // ����Ʈ ����
    public NPCName _npcName;                                          // ����Ʈ ������
    public string _title;                                             // ����Ʈ ����
    public int _questCode;                                            // ����Ʈ ������ȣ
    public string _description;                                       // ����Ʈ ����
    public List<QuestGoal> _questGoals = new List<QuestGoal>();       // ����Ʈ ��ǥ �׷�
    public Reward _reward;                                            // ����Ʈ �Ϸ� ����

    // Dialog
    public List<string> _talk = new List<string>();                   // NPC ��ȭ
    public int _openQuestIdx;                                         // �ش� talk idx���� ���� ��ȭ�� �Ѿ�� ����Ʈ â ����

    // Option
    public List<ConditionPrecedent> _conditions = new List<ConditionPrecedent>();    // ���� ����Ʈ ����
    public bool _hasConditions;                                        // ���� ����Ʈ ����
    public bool _autoComplete;                                         // ����Ʈ �ڵ� �Ϸ�
    public bool _detailFolded;                                         // GUI �󼼼��� ����
    public bool _talkFolded;                                           // GUI npc ��ȭ���� ����

    // Event
    public System.Action _nextQuest;                                   // QuestGiver�� ���� ����Ʈ �Ϸ�� ���� ����Ʈ�� ����
    public System.Action<int, QuestState> _onNPCMarker;                // QuestGiver�� ���� ����Ʈ ���¿� ���� Marker ������Ʈ

    // ��� GoalState�� Complete��� true ��ȯ
    public bool _IsAllGoalCompleted => _questGoals.All(x => x._goalState == GoalState.Complete);

    // ��� ���� ����Ʈ�� �Ϸ� �Ǹ� true ��ȯ
    public bool _IsAllQuestCompleted => _conditions.All(x => x._isCompleted);

    // ����Ʈ �޼��� ������Ʈ
    public void ReceiveReport(string target, int counting)
    {
        foreach (var goal in _questGoals)
        {
            if (!IsTarget(goal, target)) { continue; }
            goal._currentTargetCount += counting;

            // ��ǥ�� �޼��� QuestGoal�� State �Ϸ� ���·� ��ȯ
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

    // ����Ʈ�� Target ���� Ȯ��
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

    // ����Ʈ ������ȣ �ʱ�ȭ
    public void InitCode()
    {
        _questCode += (int)_npcName * 100;
    }

    // ���� ���� Ȯ���Ͽ� ����Ʈ ���� ���� ���·� ��ȯ
    public void CompleteAllCondition( )
    {
        for(int i = 0; i < _conditions.Count; i++)
        {
            if (!_conditions[i]._isCompleted) { return; }
        }

        Avaliable();
    }

    // ����Ʈ �Ϸ� ���� ȹ��
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

    #region ����Ʈ ���� ��ȯ
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


// ���� ������ ������� ����
[System.Serializable]
public class ConditionPrecedent
{
    public int _code;                   // ���� ����Ʈ�� ������ȣ
    public bool _isCompleted;           // �Ϸ� ����

    public void UpdateConditions(int code)
    {
        if (_code == code)
        {
            _isCompleted = true;
        }
    }
}
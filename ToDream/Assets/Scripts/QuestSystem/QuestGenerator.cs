using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ʈ�� ����, ���, ��ҿ���
/// ��Ͻ� QuestManager._quests�� �߰�
/// </summary>
public class QuestGenerator : MonoBehaviour
{
    public Quest _quest;

    public int _talkBoxIdx;           // talk ���� Idx (TalkList.RemoveAt)
    public int _insertIdx;            // Quest ���� Idx (QuestList.Insert)
    public int _previousIdx;          // Edit�� Idx ��ġ
    public string _insertStr;         // Edit�� Idx ��ġ string ǥ��
    public bool _enable;              // GUI - FoldOut


    #region Main Button
    public void CreateQuest()
    {
        _quest = new Quest();
        _insertIdx = QuestManager._Instance._questContainer.Count;
        _insertStr = "[ This is new one ] ";
        AddQuestGoal();
        _enable = true;
    }

    public void RegisterQuest()
    {
        if (_insertIdx > QuestManager._Instance._questContainer.Count)
        {
            Debug.LogWarning("Insert idx set from " + _insertIdx + " to " + QuestManager._Instance._questContainer.Count);
            _insertIdx = QuestManager._Instance._questContainer.Count;
        }

        QuestManager._Instance._questContainer.Insert(_insertIdx, _quest);
        Cancel();
    }

    public void Cancel()
    {
        _talkBoxIdx = 0;
        _insertIdx = QuestManager._Instance._questContainer.Count;
        _quest = null;
        _enable = false;
    }
    #endregion

    #region Talk Button
    public void AddTalk()
    {
        _quest._talk.Add("");
    }

    public void RemoveAtTalk()
    {
        if (_talkBoxIdx >= _quest._talk.Count)
        {
            Debug.LogWarning("QuestGenerator.cs -> Index out of List");
            return;
        }
        
        _quest._talk.RemoveAt(_talkBoxIdx);
    }
    #endregion

    #region QuestGoal Button
    public void AddQuestGoal()
    {
        QuestGoal questGoal = new QuestGoal();
        _quest._questGoals.Add(questGoal);
    }

    public void RemoveQuestGoal()
    {
        int last = _quest._questGoals.Count - 1;

        // QuestGoal�� �� �ϳ� ���������� '-'�� �õ��Ͽ������
        if (last == 0) 
        {
            Debug.LogWarning("You can`t remove last one of QuestGoal");
            return;
        }
        _quest._questGoals.RemoveAt(last);
    }
    #endregion

    #region Codition Button
    public void AddCondition()
    {
        ConditionPrecedent condition = new ConditionPrecedent();
        _quest._conditions.Add(condition);
    }

    public void RemoveCondition()
    {
        int last = _quest._conditions.Count - 1;

        // ���� ���� Indext�� �ϳ��� ���� �� '-'�� ������ ���
        if (last <= -1)
        {
            Debug.LogWarning("The Conditions don't have index anymore");
            return;
        }

        _quest._conditions.RemoveAt(last);

        // �� �� �߰��� ���¿��� Indext�� �ϳ��� ���� �Ǹ� Condition üũ �ڵ� ����
        if (last == 0)
        {
            _quest._hasConditions = false;
        }

    }
    #endregion

    // Edit : �ٽ� ������� �̵�
    public void Edit(Quest quest)
    {
        _quest = quest;
        _previousIdx = QuestManager._Instance._questContainer.IndexOf(quest);
        QuestManager._Instance._questContainer.Remove(quest);
        _insertIdx = QuestManager._Instance._questContainer.Count;
        _insertStr = "[ Previous Idx " + _previousIdx + " ]";
        _enable = true;
    }
}

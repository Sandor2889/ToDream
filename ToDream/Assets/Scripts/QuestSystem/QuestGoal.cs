using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GoalState
{
    InProgress,     // ������
    Complete        // ��ǥ�� �޼�
}

[System.Serializable]
public class QuestGoal
{
    public GoalState _goalState;                          // ���� ����
    public string _target;                                // ����Ʈ ��ǥ
    public string _subTitle;                              // ���� Ÿ��Ʋ
    public int _currentTargetCount;                       // ���� ����
    public int _requiredAmount;                           // �䱸 ����
    public QuestTargetMarker _targetMarker;               // ����Ʈ ��ǥ ���� ��Ŀ Ȱ��ȭ
    public bool _isFolded;                                // GUI ���� ���


    // GoalState ��ȯ - �Ϸ� 
    public void Complete()
    {
        _goalState = GoalState.Complete;
        OffQuestMarker();
    }

    // ����Ʈ ��ҽ�
    public void Cancel()
    {
        _goalState = GoalState.InProgress;
        OffQuestMarker();
        _currentTargetCount = 0;
    }

    // ����Ʈ�� �����ϸ� ����Ʈ ��Ŀ Ȱ��ȭ
    public void OnQuestMarker()
    {
        _targetMarker.gameObject.SetActive(true);
    }

    public void OffQuestMarker()
    {
        _targetMarker.gameObject.SetActive(false);
    }
}

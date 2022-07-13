using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion // 퀘스트 완료 대기 (자동완료 or 수동완료)
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskCountedHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceledHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion

    [SerializeField] private Category _category;
    [SerializeField] private Sprite _icon;

    [Header("[ Text ]")]
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private string _description;

    [Header("[ Task ]")]
    [SerializeField] private TaskGroup[] _taskGroups;           // 연속 퀘스트

    [Header("[ Reward ]")]
    [SerializeField] private Reward[] _rewards;                 // 퀘스트 보상

    [Header("[ Option ]")]
    [SerializeField] private bool _useAutoComplete;             // 퀘스트 자동 완료 설정
    [SerializeField] private bool _isCancelable;

    [Header("[ Condition ]")]
    [SerializeField] private Condition[] _acceptionConditions;  // 선행 퀘스트 조건
    [SerializeField] private Condition[] _cancelConditions;     // 퀘스트 취소 조건

    private int _currentTaskGroupIdx;

    public Category _Category => _category;
    public Sprite _Icon => _icon;
    public string _ID => _id;
    public string _DisplayName => _displayName;
    public string _Description => _description;

    public QuestState _State { get; private set; }

    public TaskGroup _CurrentTaskGroup => _taskGroups[_currentTaskGroupIdx];
    public IReadOnlyList<TaskGroup> _TaskGroups => _taskGroups;
    public IReadOnlyList<Reward> _Rewards => _rewards;
    public bool _IsRegistered => _State != QuestState.Inactive;
    public bool _IsCompletable => _State == QuestState.WaitingForCompletion;
    public bool _IsComplete => _State == QuestState.Complete;
    public bool _IsCancel => _State == QuestState.Cancel;
    public bool _IsCancelable => _isCancelable && _cancelConditions.All(x => x.IsPass(this));
    public bool _IsAcceptable => _acceptionConditions.All(x => x.IsPass(this));

    public event TaskCountedHandler _onTaskCounted;
    public event CompletedHandler _onCompleted;
    public event CanceledHandler _onCanceled;
    public event NewTaskGroupHandler _onNewTaskGroup;

    // 퀘스트가 System에 등록되었을때 실행
    public void OnRegister()
    {
        Debug.Assert(!_IsRegistered, "This quest has already been registered.");

        foreach (var taskGroup in _taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup._Tasks)
            {
                task._onCountedChanged += OnCounted;
            }
        }

        _State = QuestState.Running;
        _CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(_IsRegistered, "This quest has already been registered.");
        Debug.Assert(!_IsCancel, "This quest has been canceled.");

        if(_IsComplete) { return; }

        _CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (_CurrentTaskGroup._IsAllTaskComplete)
        {
            if (_currentTaskGroupIdx + 1 == _taskGroups.Length)
            {
                _State = QuestState.WaitingForCompletion;
                if (_useAutoComplete) { Complete(); }
            }
            else
            {
                var prevTaskGroup = _taskGroups[_currentTaskGroupIdx++];
                prevTaskGroup.End();
                _CurrentTaskGroup.Start();
                _onNewTaskGroup?.Invoke(this, _CurrentTaskGroup, prevTaskGroup);
            }
        }
        else { _State = QuestState.Running; }
    }

    public void Complete()
    {
        //CheckIsRunning();

        foreach (var taskGroup in _taskGroups)
        {
            taskGroup.Complete();    
        }

        _State = QuestState.Complete;

        foreach (var reward in _rewards)
        {
            reward.Give(this);
        }

        _onCompleted?.Invoke(this);

        _onTaskCounted = null;
        _onCompleted = null;
        _onCanceled = null;
        _onNewTaskGroup = null;
    }

    public void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(_IsCancelable, "This quest can't be canceled");

        _State = QuestState.Cancel;
        _onCanceled?.Invoke(this);
    }

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone._taskGroups = _taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void OnCounted(Task task, int currentCount, int prevCount)
        => _onTaskCounted?.Invoke(this, task, currentCount, prevCount);

    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(_IsRegistered, "This quest has already been registered.");
        Debug.Assert(!_IsCancel, "This quest has been canceled.");
        Debug.Assert(!_IsCompletable, "This quest has already been completed");
    }
}

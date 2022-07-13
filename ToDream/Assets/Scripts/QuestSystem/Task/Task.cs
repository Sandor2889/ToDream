using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangedHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void CountedHandler(Task task, int currentCount, int prevCount);
    #endregion

    [SerializeField] private Category _category;

    [Header("[ Text ]")]
    [SerializeField] private string _id;
    [SerializeField] private string _description;

    [Header("[ Action ]")]
    [SerializeField] private TaskAction _action;

    [Header("[ Target ]")]
    [SerializeField] private TaskTarget[] _targets;

    [Header("[ Setting ]")]
    [SerializeField] private InitalSuccessValue _initalSuccessValue;
    [SerializeField] private int _requireAmount;                        // 필요 수량
    [SerializeField] private bool canReceiveReportsDuringCompletion;    // 완료 조건이 갖춰지고 아이템을 버렸을 경우 다시 갱신시키기

    private TaskState _state;
    private int _currentCount;

    public event StateChangedHandler _onStateChanged;
    public event CountedHandler _onCountedChanged;

    public int _CurrentCount 
    { 
        get => _currentCount;
        set
        {
            int prevCount = _currentCount;
            _currentCount = Mathf.Clamp(value, 0, _requireAmount);
            if (_currentCount != prevCount)
            {
                _state = _currentCount == _requireAmount ? TaskState.Complete : TaskState.Running;
                _onCountedChanged?.Invoke(this, _currentCount, prevCount);
            }
        }
    }

    public Category _Category => _category;

    public string _ID => _id;
    public string _Description => _description;
    public int _RequireAmount => _requireAmount;

    public TaskState _State
    {
        get => _state;
        set
        {
            var prevState = _state;
            _state = value;
            _onStateChanged?.Invoke(this, _state, prevState);
        }
    }

    public bool _IsComplete => _State == TaskState.Complete;
    public Quest _Owner { get; private set; }

    public void Setup(Quest owner)
    {
        _Owner = owner;
    }

    public void Start()
    {
        _state = TaskState.Running;
        if (_initalSuccessValue) { _CurrentCount = _initalSuccessValue.GetValue(this); }
    }

    public void End()
    {
        _onStateChanged = null;
        _onCountedChanged = null;
        Debug.Log(_ID + ": " + _State);
    }

    public void ReceiveReport(int successCount)
    {
        _CurrentCount = _action.Run(this, _CurrentCount, successCount);
    }

    public void Complete()
    {
        _CurrentCount = _requireAmount;
    }

    /// <summary>
    /// Task 함수를 통해 이 Task가 성공 횟수를 보고 받을 대상인지 확인하는 함수
    /// </summary>
    /// <param name="target"></param>
    /// <returns> Setting 해놓은 Target들 중에 해당하는 Target이 있으면 true 없으면, false 반환 </returns>
    public bool IsTarget(string category, object target) 
        => _Category  == category &&
        _targets.Any(x => x.IsEqual(target)) &&
        (!_IsComplete || (_IsComplete && canReceiveReportsDuringCompletion));

}

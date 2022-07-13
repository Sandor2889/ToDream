using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskGroupState
{
    inactive,
    Running,
    Complete
}

[System.Serializable]
public class TaskGroup
{
    [SerializeField] private Task[] _tasks;

    public IReadOnlyList<Task> _Tasks => _tasks;

    public Quest _Owner { get; private set; }

    public bool _IsAllTaskComplete => _tasks.All(x => x._IsComplete);
    public bool _IsComplete => _Staste == TaskGroupState.Complete;

    public TaskGroupState _Staste { get; private set; }

    public TaskGroup(TaskGroup copyTarget)
    {
        _tasks = copyTarget._Tasks.Select(x => Object.Instantiate(x)).ToArray();
    }

    public void Setup(Quest owner)
    {
        _Owner = owner;
        foreach (var task in _tasks)
        {
            task.Setup(owner);
        }
    }

    // Task 그룹의 시작
    public void Start()
    {
        _Staste = TaskGroupState.Running;
        foreach (var task in _tasks)
        {
            task.Start();
        }
    }

    // Task 그룹의 종료
    public void End()
    {
        foreach (var task in _tasks)
        {
            task.End();
        }
    }

    // Task 그룹의 진행도 갱신
    public void ReceiveReport(string category, object target, int successCount)
    {
        foreach (var task in _tasks)
        {
            if (task.IsTarget(category, target))
            {
                task.ReceiveReport(successCount);
            }
        }
    }

    // Task 그룹의 완료
    public void Complete()
    {
        if (_IsComplete) { return; }

        _Staste = TaskGroupState.Complete;

        foreach (var task in _tasks)
        {
            if (!task._IsComplete) { task.Complete(); }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 들어온 성공값이 양수일땜나 Count
/// </summary>
[CreateAssetMenu(menuName = "Quest/Task/Action/PositiveCount", fileName = "Positive Count")]
public class PositiveCount : TaskAction
{
    public override int Run(Task task, int currentCount, int successCount)
    {
        return successCount > 0 ? currentCount + successCount : currentCount;
    }
}

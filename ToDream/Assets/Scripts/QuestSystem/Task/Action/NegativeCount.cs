using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 들어온 성공값이 음수 일때만 Count
/// </summary>
[CreateAssetMenu(menuName = "Quest/Task/Action/NegativeCount", fileName = "Negative Count")]
public class NegativeCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        return successCount < 0 ? currentSuccess - successCount : currentSuccess;
    }
}

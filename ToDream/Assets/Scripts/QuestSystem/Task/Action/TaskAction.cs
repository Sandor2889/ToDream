using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskAction : ScriptableObject
{
    // Run(현재 임무, 현재수량, 성공시 증가량)
    public abstract int Run(Task task, int currentCount, int successCount); 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskTarget : ScriptableObject
{
    public abstract object _Value { get; }
    public abstract bool IsEqual(object target);
}

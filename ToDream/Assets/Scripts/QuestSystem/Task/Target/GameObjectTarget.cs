using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField] private GameObject _value;

    public override object _Value => _value;

    public override bool IsEqual(object target)
    {
        var targetAsGameObject = target as GameObject;
        if (targetAsGameObject == null) { return false; }

        // 오브젝트 이름을 전혀 다른 이름으로 수정하게되면 찾지 못함
        return targetAsGameObject.name.Contains(_value.name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Category", fileName = "Category_")]
public class Category : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;

    public string _ID => _id;
    public string _DisplayName => _displayName;

    public bool IsEquals(Category other)
    {
        if (other is null) { return false; }                    // null 확인
        if (ReferenceEquals(other, this)) { return true; }      // 주소 확인
        if (GetType() != other.GetType()) { return false; }     // 타입 확인

        return _id == other._ID;
    }

    public override int GetHashCode() => (_ID, _DisplayName).GetHashCode();

    public override bool Equals(object other) => base.Equals(other);

    public static bool operator ==(Category category, string str)
    {
        if (category is null) { return ReferenceEquals(str, null); }

        return category._ID == str || category._DisplayName == str;
    }

    public static bool operator !=(Category category, string str) => !(category == str);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDataBase))]
public class ItemDataBaseEditor : Editor
{
    private ItemDataBase _itemDataBase;
    private bool _isFolded;              // "Show data" GUI 접기기능

    private void OnEnable()
    {
        _itemDataBase = target as ItemDataBase;
    }

    public override void OnInspectorGUI()
    {
        // Dictionary 띄우기
        GUILayout.BeginVertical("box");
        if (_isFolded = EditorGUILayout.Foldout(_isFolded, "Show data"))
        {
            for (int i = 0; i < _itemDataBase._itemDic.Count; i++)
            {
                string str = "[" + i + "] Item data";

                GUILayout.BeginHorizontal("box");
                GUI.enabled = false;
                _itemDataBase._itemDic.g_InspectorKeys[i] = EditorGUILayout.IntField(str, _itemDataBase._itemDic.g_InspectorKeys[i]);
                _itemDataBase._itemDic.g_InspectorValues[i] = (Item)EditorGUILayout.ObjectField(_itemDataBase._itemDic.g_InspectorValues[i], typeof(Item), true);
                GUI.enabled = true;
                if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(20)))
                {
                    _itemDataBase._itemDic.Remove(_itemDataBase._itemDic.g_InspectorKeys[i]);
                }
                GUILayout.EndHorizontal();
            }
        }
        
        GUILayout.EndVertical();

        GUILayout.Space(20);

        // 등록하고자하는 item data 넣기
        _itemDataBase._inputItemData = (Item)EditorGUILayout.ObjectField("Input Item data: ", _itemDataBase._inputItemData, typeof(Item), true);
        
        if (GUILayout.Button("Register", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (_itemDataBase._inputItemData == null)
            {
                Debug.Log("Input data is empty");
                return;
            }

            _itemDataBase.RegisterItem(_itemDataBase._inputItemData._key, _itemDataBase._inputItemData);      
        }
    }
}

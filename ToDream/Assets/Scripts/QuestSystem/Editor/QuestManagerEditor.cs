using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    private QuestManager _questMgr;

    private void OnEnable()
    {
        _questMgr = target as QuestManager;
    }

    public override void OnInspectorGUI()
    {
        for (int idx = 0; idx < _questMgr._quests.Count; idx++)
        {
            string s = "[Quest " + idx + "]"; 

            GUILayout.BeginVertical("box");
            GUILayout.Label(s);
            _questMgr._quests[idx]._Title = EditorGUILayout.TextField("Title:", _questMgr._quests[idx]._Title);
            
            // Fold out
            if(_questMgr._quests[idx]._IsFolded = EditorGUILayout.Foldout(_questMgr._quests[idx]._IsFolded, "View details"))
            {
                //_questMgr._quests[idx]._questID = EditorGUILayout.IntField("ID:", _questMgr._quests[idx]._questID);
                _questMgr._quests[idx]._NpcName = EditorGUILayout.TextField("NPC:", _questMgr._quests[idx]._NpcName);
                _questMgr._quests[idx]._Target = EditorGUILayout.TextField("Target:", _questMgr._quests[idx]._Target);
                _questMgr._quests[idx]._RequireAmount = EditorGUILayout.IntField("RequireAmount:", _questMgr._quests[idx]._RequireAmount);
                _questMgr._quests[idx]._TargetMarker = (QuestTestArea)EditorGUILayout.ObjectField("TargetMarker", _questMgr._quests[idx]._TargetMarker, typeof(QuestTestArea), true);
                GUILayout.Space(20);

                if (GUILayout.Button("Remove", GUILayout.Width(60), GUILayout.Height(25)))
                {
                    _questMgr._quests.RemoveAt(idx);
                }
            }
            GUILayout.EndVertical();
        }

        EditorUtility.SetDirty(target);
    }
}

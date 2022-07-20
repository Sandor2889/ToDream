using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestGenerator))]
public class QuestGeneratorEditor : Editor
{
    private QuestGenerator _questGenerate;

    private void OnEnable()
    {
        _questGenerate = target as QuestGenerator;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Create quest", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerate.CreateQuest();
        }
        else if (GUILayout.Button("Register quest", GUILayout.Width(100), GUILayout.Height(25)) && _questGenerate._enable)
        {
            _questGenerate.RegisterQuest();
        }
        else if(GUILayout.Button("Cencel", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerate.Cancel();
        }
        GUILayout.EndHorizontal();

        if (_questGenerate._enable)
        {
            _questGenerate._quest._Title = EditorGUILayout.TextField("Title:", _questGenerate._quest._Title);
            _questGenerate._quest._NpcName = EditorGUILayout.TextField("NPC:", _questGenerate._quest._NpcName);
            //_questGenerate._quest._questID = EditorGUILayout.IntField("ID:", _questGenerate._quest._questID);
            _questGenerate._quest._Description = EditorGUILayout.TextField("Description:", _questGenerate._quest._Description);
            //_questGenerate._quest._talk = EditorGUILayout.TextArea();
            _questGenerate._quest._Target = EditorGUILayout.TextField("Target", _questGenerate._quest._Target);
            _questGenerate._quest._RequireAmount = EditorGUILayout.IntField("RequireAmount:", _questGenerate._quest._RequireAmount);
            _questGenerate._quest._TargetMarker = (QuestTestArea)EditorGUILayout.ObjectField("TargetMarker", _questGenerate._quest._TargetMarker, typeof(QuestTestArea), true);
        }
    }
}

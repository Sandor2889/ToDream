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

        if (_questGenerate._enable)
        {
            Quest quest = _questGenerate._quest;

            quest._title = EditorGUILayout.TextField("Title:", quest._title);
            quest._npcName = (NPCName)EditorGUILayout.EnumPopup("NPC:", quest._npcName);
            //_questGenerate._quest._questID = EditorGUILayout.IntField("ID:", _questGenerate._quest._questID);
            quest._description = EditorGUILayout.TextField("Description:", quest._description);
            quest._target = EditorGUILayout.TextField("Target", quest._target);
            quest._requireAmount = EditorGUILayout.IntField("RequireAmount:", quest._requireAmount);
            quest._targetMarker = (QuestTestArea)EditorGUILayout.ObjectField("TargetMarker", quest._targetMarker, typeof(QuestTestArea), true);
            quest._autoComplete = EditorGUILayout.Toggle("Auto Complete", quest._autoComplete);
            EditorGUILayout.Space(10);

            // ----------------------------------------- NPC Talk ------------------------------------------
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("[ NPC talk ]");
            if (GUILayout.Button("Add talk", GUILayout.Width(100), GUILayout.Height(20)))
            {
                _questGenerate.AddTalk();
            }
            else if (GUILayout.Button("Remove talk", GUILayout.Width(100), GUILayout.Height(20)))
            {
                _questGenerate.RemoveAtTalk();
            }
            _questGenerate._talkBoxIdx = EditorGUILayout.IntField(_questGenerate._talkBoxIdx, GUILayout.Width(25));
            GUILayout.EndHorizontal();
            quest._openQuestIdx = EditorGUILayout.IntField("Open quest idx", quest._openQuestIdx);
            GUILayout.Space(10);
            for (int i = 0; i < quest._talk.Count; i++)
            {
                GUILayout.Label("Idx : " + i);
                quest._talk[i] = EditorGUILayout.TextArea(quest._talk[i]);
            }
            GUILayout.EndVertical();

            // ------------------------------------------ Insert Idx ------------------------------------------
            GUILayout.Space(20);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("Insert idx   -----> ");
            GUILayout.Label(_questGenerate._insertStr);
            _questGenerate._insertIdx = EditorGUILayout.IntField(_questGenerate._insertIdx, GUILayout.Width(30));
            if (_questGenerate._insertIdx < 0) { _questGenerate._insertIdx = 0; }
            GUILayout.EndHorizontal();
        }

        #region Main Button
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Create quest", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerate.CreateQuest();
        }
        else if (GUILayout.Button("Register quest", GUILayout.Width(100), GUILayout.Height(25)) && _questGenerate._enable)
        {
            _questGenerate.RegisterQuest();
        }
        else if (GUILayout.Button("Cencel", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerate.Cancel();
        }
        GUILayout.EndHorizontal();
        #endregion
    }
}

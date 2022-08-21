using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestGenerator))]
public class QuestGeneratorEditor : Editor
{
    private QuestGenerator _questGenerator;

    private void OnEnable()
    {
        _questGenerator = target as QuestGenerator;
    }

    public override void OnInspectorGUI()
    {
        if (_questGenerator._enable)
        {
            GUILayout.BeginVertical("box");
            Quest quest = _questGenerator._quest;

            // ----------------------------------------- Main -------------------------------------------------
            quest._title = EditorGUILayout.TextField("Title:", quest._title);
            quest._questCode = EditorGUILayout.IntField("Code:", quest._questCode);
            quest._npcName = (NPCName)EditorGUILayout.EnumPopup("NPC:", quest._npcName);
            EditorGUILayout.LabelField("Description:");
            quest._description = EditorGUILayout.TextArea( quest._description);

            // --------------------------------------- Quest Goal --------------------------------------------
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("[ Quest Goals ]");
            if (GUILayout.Button("+", GUILayout.Width(40), GUILayout.Height(20)))
            {
                _questGenerator.AddQuestGoal();
            }
            else if (GUILayout.Button("-", GUILayout.Width(40), GUILayout.Height(20)))
            {
                _questGenerator.RemoveQuestGoal();
            }
            GUILayout.EndHorizontal();
            for (int i = 0; i < quest._questGoals.Count; i++)
            {
                quest._questGoals[i]._subTitle = EditorGUILayout.TextField(" Sub title", quest._questGoals[i]._subTitle);
                EditorGUI.indentLevel++;
                quest._questGoals[i]._target = EditorGUILayout.TextField("¡¤ Target", quest._questGoals[i]._target);
                quest._questGoals[i]._requiredAmount = EditorGUILayout.IntField("¡¤ RequireAmount:", quest._questGoals[i]._requiredAmount);
                quest._questGoals[i]._targetMarker = (QuestTargetMarker)EditorGUILayout.ObjectField("¡¤ TargetMarker", quest._questGoals[i]._targetMarker, typeof(QuestTargetMarker), true);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5);
            }
            EditorGUILayout.LabelField(" [ Option ]");
            quest._autoComplete = EditorGUILayout.ToggleLeft(" Auto Complete", quest._autoComplete);
            quest._hasConditions = EditorGUILayout.ToggleLeft(" Condition Precedent", quest._hasConditions);
            if (quest._hasConditions)
            {
                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("+", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    _questGenerator.AddCondition();
                }
                else if (GUILayout.Button("-", GUILayout.Width(40), GUILayout.Height(20)))
                {
                    _questGenerator.RemoveCondition();
                }
                GUILayout.EndHorizontal();
                for (int i = 0; i < quest._conditions.Count; i++)
                {
                    string codeStrIdx = "(" + i + ")";
                    quest._conditions[i]._code = EditorGUILayout.IntField(codeStrIdx + " Quest code:", quest._conditions[i]._code);
                }
            }
            GUILayout.EndVertical();     
            EditorGUILayout.Space(5);

            // ----------------------------------------- Reward --------------------------------------------
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(" [ Reward ]");
            quest._reward._gold = EditorGUILayout.IntField(" Gold:", quest._reward._gold);
            quest._reward._itemKey = EditorGUILayout.IntField(" Item Key:", quest._reward._itemKey);
            GUILayout.EndVertical();
            EditorGUILayout.Space(5);
            // ----------------------------------------- NPC Talk ------------------------------------------
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("[ NPC talk ]");
            if (GUILayout.Button("Add talk", GUILayout.Width(100), GUILayout.Height(20)))
            {
                _questGenerator.AddTalk();
            }
            else if (GUILayout.Button("Remove talk", GUILayout.Width(100), GUILayout.Height(20)))
            {
                _questGenerator.RemoveAtTalk();
            }
            _questGenerator._talkBoxIdx = EditorGUILayout.IntField(_questGenerator._talkBoxIdx, GUILayout.Width(25));
            GUILayout.EndHorizontal();
            quest._openQuestIdx = EditorGUILayout.IntField(" Open quest idx", quest._openQuestIdx);
            GUILayout.Space(10);
            for (int i = 0; i < quest._talk.Count; i++)
            {
                GUILayout.Label("idx [" + i + "]");
                quest._talk[i] = EditorGUILayout.TextArea(quest._talk[i]);
            }
            GUILayout.EndVertical();

            // ------------------------------------------ Insert Idx ------------------------------------------
            GUILayout.Space(8);
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(" Insert idx   -----> ");
            GUILayout.Label(_questGenerator._insertStr);
            _questGenerator._insertIdx = EditorGUILayout.IntField(_questGenerator._insertIdx, GUILayout.Width(30));
            if (_questGenerator._insertIdx < 0) { _questGenerator._insertIdx = 0; }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        #region Main Button
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Create quest", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerator.CreateQuest();
        }
        else if (GUILayout.Button("Register quest", GUILayout.Width(100), GUILayout.Height(25)) && _questGenerator._enable)
        {
            _questGenerator.RegisterQuest();
        }
        else if (GUILayout.Button("Cencel", GUILayout.Width(100), GUILayout.Height(25)))
        {
            _questGenerator.Cancel();
        }
        GUILayout.EndHorizontal();
        #endregion
    }
}

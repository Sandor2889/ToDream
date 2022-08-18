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
            string s = "============= Quest " + idx + " =============";
            Quest quest = _questMgr._quests[idx];

            GUILayout.BeginVertical("box");
            GUILayout.Label(s);
            // View details Fold out
            if(_questMgr._quests[idx]._detailFolded = EditorGUILayout.Foldout(quest._detailFolded, "Title:  " + quest._title))
            {
                quest._questCode = EditorGUILayout.IntField("Code:", quest._questCode);
                quest._npcName = (NPCName)EditorGUILayout.EnumPopup("NPC:", quest._npcName);
                EditorGUILayout.LabelField("Description:");
                quest._description = EditorGUILayout.TextArea(quest._description);

                // GUI - QuestGoal
                GUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("[ Quest Goals ]");
                for (int i = 0; i < quest._questGoals.Count; i++)
                {
                    string goalStrIdx = "(" + i + ") ";
                    if (quest._questGoals[i]._isFolded = EditorGUILayout.Foldout(quest._questGoals[i]._isFolded, goalStrIdx + quest._questGoals[i]._subTitle))
                    {
                        EditorGUI.indentLevel++;
                        quest._questGoals[i]._subTitle = EditorGUILayout.TextField("， Sub title", quest._questGoals[i]._subTitle);
                        quest._questGoals[i]._target = EditorGUILayout.TextField("， Target", quest._questGoals[i]._target);
                        quest._questGoals[i]._requiredAmount = EditorGUILayout.IntField("， RequireAmount:", quest._questGoals[i]._requiredAmount);
                        quest._questGoals[i]._targetMarker = (QuestTargetMarker)EditorGUILayout.ObjectField("， TargetMarker", quest._questGoals[i]._targetMarker, typeof(QuestTargetMarker), true);
                        EditorGUI.indentLevel--;
                    }
                    GUILayout.Space(5);
                }
                EditorGUILayout.LabelField("[ Option ]");
                quest._autoComplete = EditorGUILayout.ToggleLeft(" Auto Complete", quest._autoComplete);
                quest._hasConditions = EditorGUILayout.ToggleLeft(" Condition Precedent", quest._hasConditions);
                if (quest._hasConditions)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < quest._conditions.Count; i++)
                    {
                        string codeStrIdx = "(" + i + ")";
                        quest._conditions[i]._code = EditorGUILayout.IntField(codeStrIdx + " Quest code:", quest._conditions[i]._code);
                    }
                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();

                // GUI - Reward
                GUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("[ Reward ]");
                quest._reward._gold = EditorGUILayout.IntField("Gold:", quest._reward._gold);
                quest._reward._item = (Item)EditorGUILayout.ObjectField(" Item:", quest._reward._item, typeof(Item), true);
                GUILayout.EndVertical();
                EditorGUILayout.Space(5);

                // GUI - Talk
                GUILayout.BeginVertical("box");
                if (quest._talkFolded = EditorGUILayout.Foldout(quest._talkFolded, "[ View NPC talk ]"))
                {
                    if (quest._talk.Count == 0) { EditorGUILayout.LabelField("TalkList is Empty"); }
                    else
                    {
                        quest._openQuestIdx = EditorGUILayout.IntField("Open quest idx", quest._openQuestIdx);
                        GUILayout.Space(5);
                        for (int i = 0; i < quest._talk.Count; i++)
                        {
                            GUILayout.Label("idx [" + i + "]");
                            quest._talk[i] = EditorGUILayout.TextArea(quest._talk[i]);
                        }
                    }
                }
                GUILayout.EndVertical();

                #region Button
                GUILayout.Space(10);
                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("Remove", GUILayout.Width(60), GUILayout.Height(25)))
                {
                    _questMgr._quests.RemoveAt(idx);
                }
                else if (GUILayout.Button("Edit", GUILayout.Width(60), GUILayout.Height(25)))
                {
                    _questMgr.GetComponent<QuestGenerator>().Edit(quest);
                }
                GUILayout.EndHorizontal();
                #endregion
            }
            GUILayout.EndVertical();
        }
    }
}

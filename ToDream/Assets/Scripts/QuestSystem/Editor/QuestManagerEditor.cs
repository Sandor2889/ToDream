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
            Quest quest = _questMgr._quests[idx];

            GUILayout.BeginVertical("box");
            GUILayout.Label(s);
            // View details Fold out
            if(_questMgr._quests[idx]._detailFolded = EditorGUILayout.Foldout(quest._detailFolded, "Title:  " + quest._title))
            {
                quest._npcName = (NPCName)EditorGUILayout.EnumPopup("NPC:", quest._npcName);
                quest._description = EditorGUILayout.TextField("Description:", quest._description);

                // GUI - QuestGoal
                GUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("[ Quest Goals ]");
                for (int i = 0; i < quest._questGoals.Count; i++)
                {
                    string goalStrIdx = "(" + i + ") ";
                    if (quest._questGoals[i]._isFolded = EditorGUILayout.Foldout(quest._questGoals[i]._isFolded, goalStrIdx + quest._questGoals[i]._subTitle))
                    {
                        EditorGUI.indentLevel++;
                        quest._questGoals[i]._subTitle = EditorGUILayout.TextField("¡æ Sub title", quest._questGoals[i]._subTitle);
                        quest._questGoals[i]._target = (QuestTarget)EditorGUILayout.EnumPopup("¡æ Target", quest._questGoals[i]._target);
                        quest._questGoals[i]._requireAmount = EditorGUILayout.IntField("¡æ RequireAmount:", quest._questGoals[i]._requireAmount);
                        quest._questGoals[i]._targetMarker = (QuestTargetMarker)EditorGUILayout.ObjectField("¡æ TargetMarker", quest._questGoals[i]._targetMarker, typeof(QuestTargetMarker), true);
                        EditorGUI.indentLevel--;
                    }
                    GUILayout.Space(5);
                }
                quest._autoComplete = EditorGUILayout.Toggle("Auto Complete", quest._autoComplete);
                GUILayout.EndVertical();

                // GUI - Reward
                GUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("[ Reward ]");
                quest._reward._gold = EditorGUILayout.IntField("Gold:", quest._reward._gold);
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
                GUILayout.Space(20);
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

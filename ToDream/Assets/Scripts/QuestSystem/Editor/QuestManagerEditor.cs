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
            quest._title = EditorGUILayout.TextField("Title:", quest._title);
            // Fold out
            if(_questMgr._quests[idx]._detailFolded = EditorGUILayout.Foldout(quest._detailFolded, "View details"))
            {
                //_questMgr._quests[idx]._questID = EditorGUILayout.IntField("ID:", _questMgr._quests[idx]._questID);
                quest._npcName = EditorGUILayout.TextField("NPC:", quest._npcName);
                quest._description = EditorGUILayout.TextField("Description:", quest._description);
                quest._target = EditorGUILayout.TextField("Target:", quest._target);
                quest._requireAmount = EditorGUILayout.IntField("RequireAmount:", quest._requireAmount);
                quest._targetMarker = (QuestTestArea)EditorGUILayout.ObjectField("TargetMarker", quest._targetMarker, typeof(QuestTestArea), true);

                // GUI - Talk
                if (quest._talkFolded = EditorGUILayout.Foldout(quest._talkFolded, "[ View NPC talk ]"))
                {
                    if (quest._talk.Count == 0) { EditorGUILayout.LabelField("TalkList is Empty"); }
                    else
                    {
                        quest._openQuestIdx = EditorGUILayout.IntField("Open quest idx", quest._openQuestIdx);
                        GUILayout.Space(10);
                        for (int i = 0; i < quest._talk.Count; i++)
                        {
                            GUILayout.Label("idx : " + i);
                            quest._talk[i] = EditorGUILayout.TextArea(quest._talk[i]);
                        }
                    }
                }

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

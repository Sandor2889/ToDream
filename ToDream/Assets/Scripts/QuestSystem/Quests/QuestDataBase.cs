using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest/QuestDatabase")]
public class QuestDataBase : ScriptableObject
{
    [SerializeField] private List<Quest> _quests;

    public IReadOnlyList<Quest> _Quests => _quests;

    public Quest FindQuestBy(string questID) => _quests.FirstOrDefault(x => x._ID == questID);


#if UNITY_EDITOR
    [ContextMenu("FindQuests")]
    private void FindQuests()
    {
        FindQuestBy<Quest>();
    }

    private void FindQuestBy<T>() where T : Quest
    {
        _quests = new List<Quest>();

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            _quests.Add(quest);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int _questId;
    Dictionary<int, QuestData> _questList;

    private void Awake()
    {
        _questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        //_questList.Add(10, new QuestData("Test", new int[] { 1000, 2000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return _questId;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string _questName;
    public int[] _npcId;

    public QuestData(string name, int[] npc)
    {
        _questName = name;
        _npcId = npc;
    }
}

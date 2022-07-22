using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class QuestGiver : MonoBehaviour
{
    public List<Quest> _quests = new List<Quest>();

    public Quest _CurrentQuest => _quests[0];

    private void Awake()
    {
        DistributeQuests();
    }

    private void DistributeQuests()
    {
        QuestManager questMgr = QuestManager._Instance;

        _quests = questMgr._quests.FindAll(x => x._npcName == this.name);

        // 디버그용
        foreach (var item in _quests)
        {
            Debug.Log("The " + this.name + " has quest -> " + item._title);
        }
    }
}

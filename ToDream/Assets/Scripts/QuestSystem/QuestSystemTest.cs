using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystemTest : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    [SerializeField] private Category _category;
    [SerializeField] private TaskTarget _target;

    private void Start()
    {
        var questSystem = QuestSystem._Instance;

        questSystem._onQuestRegistered += (quest) =>
        {
            print($"New Quest : {quest._ID} Registered");
            print($"Active Quests Count : {questSystem._ActiveQuests.Count}");
        };

        questSystem._onQuestCompleted += (quest) =>
        {
            print($"Quest:{quest._ID} Completed");
            print($"Completed Quests Count: {questSystem._CompletedQuests.Count}");
        };

        var newQuest = questSystem.Register(_quest);
        newQuest._onTaskCounted += (quest, task, currentCount, prevCount) =>
        {
            print($"Quest:{quest._ID}, Task:{task._ID}, CurrentSuccess: {currentCount}");
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QuestSystem._Instance.ReceiveReport(_category, _target, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Text _title;
    public Text _description;
    //public Reward _reward;

    public void OpenQuestUI()
    {
        UIManager UIMgr = UIManager._Instance;
        Quest quest = UIMgr._ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        if (quest._questState != QuestState.Avaliable) { return; }

        SetText(quest._title, quest._description);
        gameObject.SetActive(true);
    }

    public void RefuseQuestUI()
    {
        CloseQuestUI();
        UIManager._Instance._DialogUI.CloseDialog();
    }

    public void CloseQuestUI()
    {
        gameObject.SetActive(false);
    }

    public void AcceptQuestUI()
    {
        UIManager UIMgr = UIManager._Instance;
        Quest quest = UIMgr._ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        quest.Accepted();
        QuestManager._Instance._acceptedQuests.Add(quest);
        Debug.Log("Accept Quest -> " + quest._title);
        CloseQuestUI();
        UIMgr._DialogUI.SetText(quest._talk[UIMgr._DialogUI._dialogIdx]);
        UIMgr._DialogUI._dialogIdx++;
    }
    public void SetText(string title, string desc)
    {
        _title.text = title;
        _description.text = desc;
    }
}

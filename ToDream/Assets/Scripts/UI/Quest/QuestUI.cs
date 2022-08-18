using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Text _goal;
    [SerializeField] private Text _gold;
    [SerializeField] private Text _itemName;
    [SerializeField] private Image _itemImage;

    public Button _accept;
    public Button _refuse;
    public Button _cancel;
    public Button _back;

    public void OpenQuest()
    {
        Quest quest = UIManager._Instance._ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        if (quest._questState != QuestState.Avaliable) { return; }
        ActivateButton(true);
        SetText(quest);
        gameObject.SetActive(true);
    }

    public void OpenQuestByList()
    {
        UIManager UIMgr = UIManager._Instance;
        Quest quest = UIMgr._QuestListUI._qButtons.Find(x => x._quest._questCode == UIMgr._QuestListUI._clickedQCode)._quest;
        SetText(quest);
        ActivateButton(false);
        gameObject.SetActive(true);
    }

    public void RefuseQuest()
    {
        CloseQuest();
        UIManager._Instance._DialogUI.CloseDialog();
    }

    public void CloseQuest()
    {
        gameObject.SetActive(false);
    }

    public void AcceptQuest()
    {
        UIManager UIMgr = UIManager._Instance;
        QuestGiver giver = UIMgr._ObjInter._Hit.collider.GetComponent<QuestGiver>();

        giver._CurrentQuest.Accepted();
        QuestManager._Instance._acceptedQuests.Add(giver._CurrentQuest);
        UIMgr._QButtonPool.GetObj(giver._CurrentQuest);
        CloseQuest();

        if (UIMgr._DialogUI._dialogIdx < giver._CurrentQuest._talk.Count)
        {
            UIMgr._DialogUI.SetText(giver._npcName, giver._CurrentQuest._talk[UIMgr._DialogUI._dialogIdx]);
            UIMgr._DialogUI._dialogIdx++;
        }
        else
        {
            UIMgr._DialogUI.CloseDialog();
        }
    }

    // 퀘스트를 수락전으로 되돌리기
    public void CancelQuest()
    {
        UIManager UIMgr = UIManager._Instance;
        QButtenInList qButton = UIMgr._QuestListUI._qButtons.Find(x => x._quest._questCode == UIMgr._QuestListUI._clickedQCode);
        Quest quest = qButton._quest;


        // 모든 목표 상태 초기화
        foreach (var goal in quest._questGoals)
        {
            goal.Cancel();
        }
        quest.Avaliable();  // 퀘스트 상태 되돌리기

        QuestManager._Instance._acceptedQuests.Remove(quest);
        UIMgr._QButtonPool.ReturnObj(qButton);
        Back();
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void SetText(Quest quest)
    {
        _title.text = quest._title;
        _description.text = quest._description;

        string temp = "";
        for (int i = 0; i < quest._questGoals.Count; i++)
        {
            temp += "● " + quest._questGoals[i]._target + "에게 배달하기     " +
               quest._questGoals[i]._currentTargetCount + " / " + quest._questGoals[i]._requiredAmount + "\n";
        }
        _goal.text = temp;

        // Reward Text

        _gold.text = quest._reward._gold.ToString();
        if (quest._reward._item == null)
        {
            ActivateRewardItem(false);
        }
        else
        {
            ActivateRewardItem(true);
            _itemImage.sprite = quest._reward._item._itemSprite;
            _itemName.text = quest._reward._item.name;
        }

    }

    // true -> (수락, 거절)
    // false -> (취소, 뒤로가기)
    private void ActivateButton(bool normal)
    {
        _accept.gameObject.SetActive(normal);
        _refuse.gameObject.SetActive(normal);
        _cancel.gameObject.SetActive(!normal);
        _back.gameObject.SetActive(!normal);
    }

    private void ActivateRewardItem(bool isReward)
    {
        _itemImage.gameObject.SetActive(isReward);
        _itemName.gameObject.SetActive(isReward);
    }
}

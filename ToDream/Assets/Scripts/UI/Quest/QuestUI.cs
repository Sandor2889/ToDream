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

    [HideInInspector] public QuestGiver _questGiver;
    public Button _accept;
    public Button _refuse;
    public Button _cancel;
    public Button _back;

    public void OpenQuest()
    {
        Quest quest = UIManager._Instance._QuestUI._questGiver._CurrentQuest;

        if (quest._questState != QuestState.Avaliable) { return; }  // 예외 처리

        ActivateButton(true);        // 수락/거절 버튼 활성화
        SetText(quest);              // 퀘스트 내용 업데이트
        gameObject.SetActive(true);
    }

    public void OpenQuestByList()
    {
        UIManager uiMgr = UIManager._Instance;
        // QuestListUI에서 클릭한 퀘스트의 code와 수락한 퀘스트들 중 code가 일치 하는 것 찾기
        Quest quest = uiMgr._QuestListUI._qButtons.Find(
            x => x._quest._questCode == uiMgr._QuestListUI._clickedQCode)._quest;

        ActivateButton(false);        // 포기/뒤로가기 버튼 활성화
        SetText(quest);               // 퀘스트 내용 업데이트
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
        UIManager uiMgr =UIManager._Instance;
        QuestGiver giver = uiMgr._QuestUI._questGiver;

        // QuestListUI에 등록
        // 퀘스트 수락 가능 한도가 꽉찰 시 취소
        QButtonInList qObj = uiMgr._QButtonPool.GetObj(giver._CurrentQuest);
        if (qObj == null) 
        {
            Debug.Log("Quest is Full");
            CloseQuest();
            return;
        }  
        giver._CurrentQuest.Accepted();
        qObj.SetText();
        // QuestManager의 수락한 퀘스트 List에 등록
        QuestManager._Instance._acceptedQuests.Add(giver._CurrentQuest);    
        CloseQuest();

        // 현재 진행된 대화 인덱스가 퀘스트의 총량 보다 작으면 동작 (퀘스트 수락 후 대화로 넘어감)
        // 퀘스트 수락 후 대화가 없다면 대화 종료.
        if (uiMgr._DialogUI._dialogIdx < giver._CurrentQuest._talk.Count)
        {
            uiMgr._DialogUI.SetText(giver._npcName, giver._CurrentQuest._talk[uiMgr._DialogUI._dialogIdx]);
            uiMgr._DialogUI._dialogIdx++;
        }
        else
        {
            uiMgr._DialogUI.CloseDialog();
        }
    }

    // 퀘스트를 수락전으로 되돌리기
    public void CancelQuest()
    {
        UIManager uiMgr = UIManager._Instance;
        QButtonInList qButton = uiMgr._QuestListUI._qButtons.Find(
            x => x._quest._questCode == uiMgr._QuestListUI._clickedQCode);
        Quest quest = qButton._quest;


        // 모든 목표 상태 초기화
        foreach (var goal in quest._questGoals)
        {
            goal.Cancel();
        }
        quest.Avaliable();  // 퀘스트 상태 되돌리기

        QuestManager._Instance._acceptedQuests.Remove(quest);
        uiMgr._QButtonPool.ReturnObj(qButton);
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
            temp += quest._questGoals[i]._targetDesc;
        }
        _goal.text = temp;

        // Reward Text
        Item item = GameManager.GetDicValue(quest._reward._itemKey);

        _gold.text = quest._reward._gold.ToString();
        if (item == null)
        {
            ActivateRewardItem(false);
        }
        else
        {
            ActivateRewardItem(true);
            
            _itemImage.sprite = item._sprite;
            _itemName.text = item._obj.name;
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

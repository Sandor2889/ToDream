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
        Quest quest = _questGiver._CurrentQuest;

        if (quest._questState != QuestState.Avaliable) { return; }  // ���� ó��

        ActivateButton(true);        // ����/���� ��ư Ȱ��ȭ
        SetText(quest);              // ����Ʈ ���� ������Ʈ
        gameObject.SetActive(true);
    }

    public void OpenQuestByList()
    {
        UIManager uiMgr = UIManager._Instance;
        // QuestListUI���� Ŭ���� ����Ʈ�� code�� ������ ����Ʈ�� �� code�� ��ġ �ϴ� �� ã��
        Quest quest = uiMgr._QuestListUI._qButtons.Find(
            x => x._quest._questCode == uiMgr._QuestListUI._clickedQCode)._quest;

        ActivateButton(false);        // ����/�ڷΰ��� ��ư Ȱ��ȭ
        SetText(quest);               // ����Ʈ ���� ������Ʈ
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
        // QuestListUI�� ���
        UIManager uiMgr =UIManager._Instance;
        QButtonInList qObj = uiMgr._QButtonPool.GetObj(_questGiver._CurrentQuest);

        // ����Ʈ ���� ���� �ѵ��� ���� �� ���
        if (qObj == null) 
        {
            Debug.Log("Quest is Full");
            CloseQuest();
            return;
        }
        _questGiver._CurrentQuest.Accepted();
        qObj.SetText();
        // QuestManager�� ������ ����Ʈ List�� ���
        QuestManager._Instance._acceptedQuests.Add(_questGiver._CurrentQuest);    
        CloseQuest();

        // ���� ����� ��ȭ �ε����� ����Ʈ�� �ѷ� ���� ������ ���� (����Ʈ ���� �� ��ȭ�� �Ѿ)
        // ����Ʈ ���� �� ��ȭ�� ���ٸ� ��ȭ ����.
        if (uiMgr._DialogUI._dialogIdx < _questGiver._CurrentQuest._talk.Count)
        {
            uiMgr._DialogUI.SetText(_questGiver._npcName, _questGiver._CurrentQuest._talk[uiMgr._DialogUI._dialogIdx]);
            uiMgr._DialogUI._dialogIdx++;
        }
        else
        {
            uiMgr._DialogUI.CloseDialog();
        }
    }

    // ����Ʈ�� ���������� �ǵ�����
    public void CancelQuest()
    {
        UIManager uiMgr = UIManager._Instance;
        QButtonInList qButton = uiMgr._QuestListUI._qButtons.Find(
            x => x._quest._questCode == uiMgr._QuestListUI._clickedQCode);
        Quest quest = qButton._quest;


        // ��� ��ǥ ���� �ʱ�ȭ
        foreach (var goal in quest._questGoals)
        {
            goal.Cancel();
        }
        quest.Avaliable();  // ����Ʈ ���� �ǵ�����

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

    // true -> (����, ����)
    // false -> (���, �ڷΰ���)
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

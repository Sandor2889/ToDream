using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButtenInList : MonoBehaviour
{
    public Text _titleText;
    public Text _stateText;
    public Quest _quest;

    public void SetText()
    {
        _titleText.text = _quest._title;
        _stateText.text = _quest._questState.ToString();
    }


    // QuestListUI(Ȯ��â)���� Quest Ŭ���� �ش� code �ް�
    // QuestUI ����
    public void OpenQuestByList()
    {
        UIManager._Instance._QuestListUI._clickedQCode = _quest._questCode;
        UIManager._Instance._QuestUI.OpenQuestByList();
    }
}

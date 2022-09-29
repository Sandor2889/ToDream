using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButtonInList : MonoBehaviour
{
    public Text _titleText;
    public Text _stateText;
    public Quest _quest;

    public void SetText()
    {
        _titleText.text = _quest._title;
        _stateText.text = _quest._questState.ToString();
    }

    // QuestListUI에서 Quest 클릭시 해당 Quest의 code를 받고
    // QuestUI 연다. (이 메서드는 버튼 클릭 이벤트로 호출)
    public void OpenQuestByList()
    {
        UIManager._Instance._QuestListUI._clickedQCode = _quest._questCode;
        UIManager._Instance._QuestUI.OpenQuestByList();
    }
}

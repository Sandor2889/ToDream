using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : PopupUIBase
{
    public List<QButtonInList> _qButtons = new List<QButtonInList>();
    public int _clickedQCode;

    public void UpdateList(int code)
    {
        QButtonInList qButton = _qButtons.Find(x => x._quest._questCode == code);

        // ����Ʈ�� Done�̶�� �ٽ� pool�� ��ȯ
        if (qButton._quest._questState == QuestState.Done)
        {
            UIManager._Instance._QButtonPool.ReturnObj(qButton);
        }
        else 
        {
            qButton.SetText();
        }
    }

    public override void OpenControllableUI()
    {
        UIManager.CursorVisible(true);
        UIManager._isOpendUI = true;
        gameObject.SetActive(true);
    }

    public override void CloseControllableUI()
    {
        UIManager.CursorVisible(false);
        UIManager._isOpendUI = false;
        gameObject.SetActive(false);
    }
}

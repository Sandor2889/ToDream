using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    public List<QButtenInList> _qButtons = new List<QButtenInList>();
    public int _clickedQCode;

    public void UpdateList(int code)
    {
        QButtenInList qButton = _qButtons.Find(x => x._quest._questCode == code);

        // 퀘스트가 Done이라면 다시 pool로 반환
        if (qButton._quest._questState == QuestState.Done)
        {
            UIManager._Instance._QButtonPool.ReturnObj(qButton);
        }
        else 
        {
            qButton.SetText();
        }
    }

    public void OpenList()
    {
        gameObject.SetActive(true);
    }

    public void CloseList()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private Text _dialog;

    [HideInInspector] public int _dialogIdx;

    public void OpenDialog()
    {
        UpdateDialog();

        gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        _dialogIdx = 0;
        gameObject.SetActive(false);
    }

    public void UpdateDialog()
    {
        UIManager UIMgr = UIManager._Instance;
        Quest quest = UIMgr._ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        // 퀘스트 수락 전 대화
        if (_dialogIdx == quest._openQuestIdx + 1)
        {
            UIMgr._QuestUI.OpenQuestUI();
        }
        // 퀘스트 수락 후 작별 인사
        else if (_dialogIdx >= quest._talk.Count)
        {
            CloseDialog();
        }
        else
        {
            string str = quest._talk[_dialogIdx];
            SetText(str);

            _dialogIdx++;
        }
    }

    public void SetText(string msg)
    {
        _dialog.text = msg;
    }
}

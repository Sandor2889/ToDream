using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager _Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                
                if (_instance == null)
                {
                    Debug.Log("Did not find UIManager");
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField] private DialogUI _dialogUI;
    [SerializeField] private QuestUI _questUI;
    [SerializeField] private ObjectInteraction _objInter;

    private int _dialogIdx;

    public DialogUI _DialogUI => _dialogUI;
    public QuestUI _QuestUI => _questUI;
    public ObjectInteraction _ObjInter => _objInter;

    private void Awake()
    {
        _instance = this;

        _objInter = FindObjectOfType<ObjectInteraction>();
    } 

    // -------------------------------- Quest -------------------------------------//
    public void OpenQuestUI()
    {
        Quest quest = _objInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        if (quest._questState != QuestState.Avaliable) { return; }

        _questUI.SetText(quest._title, quest._description);
        _questUI.gameObject.SetActive(true);
    }

    public void RefuseQuestUI()
    {
        CloseQuestUI();
        CloseDialog();
    }

    public void CloseQuestUI()
    {
        _questUI.gameObject.SetActive(false);
    }

    public void AcceptQuestUI()
    {
        Quest quest = _ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;
        quest.Accepted();
        QuestManager._Instance._acceptedQuests.Add(quest);
        Debug.Log("Accept Quest -> " + quest._title);
        CloseQuestUI();
        _dialogUI.SetText(quest._talk[_dialogIdx]);
        _dialogIdx++;
    }

    // ---------------------------------- Dialog -----------------------------------//
    public void OpenDialog()
    {
        UpdateDialog();

        _dialogUI.gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        _dialogIdx = 0;
        _dialogUI.gameObject.SetActive(false);
    }

    public void UpdateDialog()
    {
        Debug.Log(_dialogIdx);
        Quest quest = _ObjInter._Hit.collider.GetComponent<QuestGiver>()._CurrentQuest;

        // 퀘스트 수락 전 대화
        if (_dialogIdx == quest._openQuestIdx + 1) 
        {
            OpenQuestUI();
        }
        // 퀘스트 수락 후 작별 인사
        else if (_dialogIdx >= quest._talk.Count)
        {
            Debug.Log("!");
            CloseDialog();
        }
        else
        {
            string str = quest._talk[_dialogIdx];
            _dialogUI.SetText(str);

            _dialogIdx++;
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private Text _dialog;                   // NPC 대화 텍스트
    [SerializeField] private Text _npcName;                  // NPC 이름 텍스트
    [SerializeField] private Text _nextText;                 // 다음 텍스트
    [SerializeField] private Button _nextButton;             // 다음 버튼 

    [SerializeField] private float _typingSpeed = 0.1f;      // 대화 텍스트 출력 속도
    [SerializeField] private float _effectSpeed = 0.5f;      // 다음 텍스트 효과 속도 

    private Coroutine _preCort;                              // 이전 출력중인 텍스트의 코루틴

    [HideInInspector] public int _dialogIdx;

    public void OpenDialog()
    {
        gameObject.SetActive(true);
        UIManager.CursorVisible(true);
        UIManager._Instance._InterUI.OffInterText();
        StartCoroutine(C_CloseDialog());
        UpdateDialog();
        StartCoroutine(C_NextTextEffect());
    }

    public void CloseDialog()
    {
        _dialogIdx = 0;
        gameObject.SetActive(false);
        UIManager.CursorVisible(false);
    }

    public void UpdateDialog()
    {
        UIManager uiMgr = UIManager._Instance;
        QuestGiver giver = UIManager._Instance._QuestUI._questGiver;

        // 퀘스트 창 Open 시점 (해당 Idx의 Text가 출력이 되야 하기 때문에 +1)
        if (_dialogIdx == giver._CurrentQuest._openQuestIdx + 1)
        {
            uiMgr._QuestUI.OpenQuest();
        }
        // 퀘스트 수락 후 작별 인사
        else if (_dialogIdx >= giver._CurrentQuest._talk.Count)
        {
            CloseDialog();
        }
        // 퀘스트 수락 전 대화 (dialogIdx == 0 ~ openQuestIdx - 1)
        else
        {
            string str = giver._CurrentQuest._talk[_dialogIdx];
            SetText(giver._npcName, str);
            _dialogIdx++;
        }
    }

    public void SetText(NPCName npc, string msg)
    {
        _npcName.text = npc.ToString();
        if (_preCort != null) { StopCoroutine(_preCort); }  // 첫 실행 예외처리
        _preCort = StartCoroutine(C_TypingEffect(msg));
    }

    public IEnumerator C_CloseDialog()
    {
        while (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !UIManager._Instance._QuestUI.gameObject.activeSelf)
            {
                CloseDialog();
            }
            yield return null;
        }
    }

    private IEnumerator C_TypingEffect(string msg)
    {
        _dialog.text = "";
        foreach (char letter in msg.ToCharArray())
        {
            _dialog.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }

    private IEnumerator C_NextTextEffect()
    {
        while(gameObject.activeSelf)
        {
            _nextText.text = "Next";
            yield return new WaitForSeconds(_effectSpeed);

            _nextText.text = " ";
            yield return new WaitForSeconds(_effectSpeed);
        }
    }
}

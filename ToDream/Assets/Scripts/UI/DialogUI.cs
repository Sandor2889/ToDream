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
    [SerializeField] private float _effectSpeed = 0.3f;      // 다음 텍스트 효과 속도 

    private Coroutine _preCort;                              // 이전 출력중인 텍스트의 코루틴

    [HideInInspector] public int _dialogIdx;



    public void OpenDialog()
    {
        gameObject.SetActive(true);
        StartCoroutine(C_CloseDialog());
        UIManager._Instance._InterUI.OffInterText();
        UpdateDialog();
        StartCoroutine(C_NextTextEffect());
    }

    public void CloseDialog()
    {
        _dialogIdx = 0;
        gameObject.SetActive(false);
    }

    public void UpdateDialog()
    {
        UIManager UIMgr = UIManager._Instance;
        QuestGiver giver = UIMgr._ObjInter._Hit.collider.GetComponent<QuestGiver>();

        // 퀘스트 수락 전 대화
        if (_dialogIdx == giver._CurrentQuest._openQuestIdx + 1)
        {
            UIMgr._QuestUI.OpenQuest();
        }
        // 퀘스트 수락 후 작별 인사
        else if (_dialogIdx >= giver._CurrentQuest._talk.Count)
        {
            CloseDialog();
        }
        else
        {
            string str = giver._CurrentQuest._talk[_dialogIdx];
            SetText(giver._npcName, str);

            _dialogIdx++;
        }
    }

    public void SetNextButton(bool enable, float alpha)
    {
        _nextButton.enabled = enable;
        Color color = _nextButton.GetComponent<Image>().color;
        color.a = alpha;
        _nextButton.GetComponent<Image>().color = color;
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
            _nextText.text = "▷▷ Next";
            yield return new WaitForSeconds(_effectSpeed);

            _nextText.text = "▶▷ Next";
            yield return new WaitForSeconds(_effectSpeed);

            _nextText.text = "▶▶ Next";
            yield return new WaitForSeconds(_effectSpeed);
        }
    }
}

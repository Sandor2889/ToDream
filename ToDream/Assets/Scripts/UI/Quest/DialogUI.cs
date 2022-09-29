using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private Text _dialog;                   // NPC ��ȭ �ؽ�Ʈ
    [SerializeField] private Text _npcName;                  // NPC �̸� �ؽ�Ʈ
    [SerializeField] private Text _nextText;                 // ���� �ؽ�Ʈ
    [SerializeField] private Button _nextButton;             // ���� ��ư 

    [SerializeField] private float _typingSpeed = 0.1f;      // ��ȭ �ؽ�Ʈ ��� �ӵ�
    [SerializeField] private float _effectSpeed = 0.5f;      // ���� �ؽ�Ʈ ȿ�� �ӵ� 

    private Coroutine _preCort;                              // ���� ������� �ؽ�Ʈ�� �ڷ�ƾ

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

        // ����Ʈ â Open ���� (�ش� Idx�� Text�� ����� �Ǿ� �ϱ� ������ +1)
        if (_dialogIdx == giver._CurrentQuest._openQuestIdx + 1)
        {
            uiMgr._QuestUI.OpenQuest();
        }
        // ����Ʈ ���� �� �ۺ� �λ�
        else if (_dialogIdx >= giver._CurrentQuest._talk.Count)
        {
            CloseDialog();
        }
        // ����Ʈ ���� �� ��ȭ (dialogIdx == 0 ~ openQuestIdx - 1)
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
        if (_preCort != null) { StopCoroutine(_preCort); }  // ù ���� ����ó��
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

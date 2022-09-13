using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionObjectType
{
    Default,
    NPC,
    Item
}

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private bool _canInteract;

    [SerializeField] private RaycastHit _hit;
    [SerializeField] private LayerMask _mask;
    private InteractionObjectType _interObjType;
    private int _npc;

    private void Awake()
    {
        _npc = LayerMask.NameToLayer("NPC");
    }

    private void Update()
    {
        OnInterection();
        InteractObject();
    }

    // ��ȣ�ۿ� ������ ������Ʈ Ž��
    private void OnInterection()
    {
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.blue, 1f);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 1f, _mask))
        {
            _canInteract = true;

            // hit.Layer�� NPC �϶� (�ٸ� Ÿ���� �߰� �ɰ�� else if�Ǵ� switch�� �߰�)
            if (_hit.collider.gameObject.layer == _npc)
            {
                _interObjType = InteractionObjectType.NPC;
            }

            // ��ȣ�ۿ� UI ǥ��
            if (!UIManager._Instance._InterUI.gameObject.activeSelf)
            {
                if (_interObjType == InteractionObjectType.NPC && 
                    !UIManager._Instance._DialogUI.gameObject.activeSelf &&
                    _hit.collider.GetComponent<QuestGiver>()._CurrentQuest != null &&
                    (_hit.collider.GetComponent<QuestGiver>()._CurrentQuest._questState == QuestState.Avaliable ||
                    _hit.collider.GetComponent<QuestGiver>()._CurrentQuest._questState == QuestState.Completed))
                { 
                    UIManager._Instance._InterUI.OnInterText();
                    UIManager._Instance._QuestUI._questGiver = _hit.collider.GetComponent<QuestGiver>();
                }
            }
            UIManager._Instance._InterUI.SetText(_interObjType);
        }
        else
        {
            if (_canInteract) { _canInteract = false; }

            if (UIManager._Instance._InterUI.gameObject.activeSelf)
            {
                UIManager._Instance._InterUI.OffInterText();
                UIManager._Instance._QuestUI._questGiver = null;
            }
        }
    }


    // ������Ʈ�� ��ȣ�ۿ�
    private void InteractObject()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canInteract)
        {
            switch (_interObjType)
            {
                case InteractionObjectType.NPC:
                    StartTalk();
                    break;
                case InteractionObjectType.Item:
                    // ����
                    break;
                default:
                    break;
            }
        }
    }

    private void StartTalk()
    {
        QuestGiver giver = _hit.collider.GetComponent<QuestGiver>();
        giver.transform.LookAt(this.transform); // �ٽ� ���ư��� �ʰ� ���� (origin ���ֱ�)

        if (giver._CurrentQuest == null || giver._CurrentQuest._questState == QuestState.Unvaliable)
        {
            // �ʿ�� ����Ʈ�� ��� ������ Default ��ȭ �߰�
            return;
        }
        else if (giver._CurrentQuest._questState == QuestState.Accepted)
        {
            // �ʿ�� ����Ʈ �������� ����� ��ȭ �߰�
            return;
        }

        // ����Ʈ ���� �Ϸ�
        if (giver._CurrentQuest._questState == QuestState.Completed)
        {
            int tempCode = giver._CurrentQuest._questCode;      // ���� ����Ʈ�� �Ѿ���� �ڵ� ����
            giver._CurrentQuest.Done();
            UIManager._Instance._QuestListUI.UpdateList(tempCode);
            return;
        }

        UIManager._Instance._DialogUI.OpenDialog();
    }
}

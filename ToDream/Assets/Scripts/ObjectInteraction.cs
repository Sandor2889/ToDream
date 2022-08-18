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
    [SerializeField] private QuestUI _questUI;
    [SerializeField] private bool _canInteract;

    [SerializeField] private RaycastHit _hit;
    [SerializeField] private LayerMask _mask;
    private InteractionObjectType _interObjType;

    private int _npc;

    public RaycastHit _Hit => _hit;

    private void Awake()
    {
        _npc = LayerMask.NameToLayer("NPC");
    }

    private void Update()
    {
        OnInterection();
        InteractObject();
    }

    // 상호작용 가능한 오브젝트 탐지
    private void OnInterection()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.blue, 1f);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 1f, _mask))
        {
            _canInteract = true;

            // hit.Layer가 NPC 일때
            if (_hit.collider.gameObject.layer == _npc)
            {
                _interObjType = InteractionObjectType.NPC;
            }

            // 상호작용 UI 표시
            if (!UIManager._Instance._InterUI.gameObject.activeSelf)
            {
                if (_interObjType == InteractionObjectType.NPC && 
                    !UIManager._Instance._DialogUI.gameObject.activeSelf &&
                    _hit.collider.GetComponent<QuestGiver>()._CurrentQuest != null &&
                    (_hit.collider.GetComponent<QuestGiver>()._CurrentQuest._questState == QuestState.Avaliable ||
                    _hit.collider.GetComponent<QuestGiver>()._CurrentQuest._questState == QuestState.Completed))
                {
                    
                    UIManager._Instance._InterUI.OnInterText();
                }
            }
            UIManager._Instance._InterUI.SetText(_interObjType);
        }
        else
        {
            _canInteract = false;
            if (UIManager._Instance._InterUI.gameObject.activeSelf)
            {
                UIManager._Instance._InterUI.OffInterText();
            }
        }
    }


    // 오브젝트와 상호작용
    private void InteractObject()
    {
        if (_canInteract && Input.GetKeyDown(KeyCode.F1))
        {
            switch (_interObjType)
            {
                case InteractionObjectType.NPC:
                    StartTalk();
                    break;
                case InteractionObjectType.Item:
                    // 실행
                    break;
                default:
                    Debug.Log("Somthing wrong value -> " + _interObjType);
                    break;
            }
        }
    }

    private void StartTalk()
    {
        QuestGiver giver = _hit.collider.GetComponent<QuestGiver>(); 

        if (giver._CurrentQuest == null || giver._CurrentQuest._questState == QuestState.Unvaliable)
        {
            Debug.Log("This NPC Doesn`t have any Quest");
            return;
        }
        else if (giver._CurrentQuest._questState == QuestState.Accepted)
        {
            // 필요시 퀘스트 진행중일 경우의 대화 추가
            return;
        }

        // 퀘스트 수동 완료
        if (giver._CurrentQuest._questState == QuestState.Completed)
        {
            int tempCode = giver._CurrentQuest._questCode;      // 다음 퀘스트로 넘어가기전 코드 저장
            giver._CurrentQuest.Done();
            UIManager._Instance._QuestListUI.UpdateList(tempCode);
            return;
        }

        UIManager._Instance._DialogUI.OpenDialog();
    }
}

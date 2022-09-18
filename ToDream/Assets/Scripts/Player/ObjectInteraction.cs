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
    [SerializeField] private float _rotateSpeed;
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

    // 상호작용 가능한 오브젝트 탐지
    private void OnInterection()
    {
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.blue, 1f);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 1f, _mask))
        {
            _canInteract = true;

            // hit.Layer가 NPC 일때 (다른 타입이 추가 될경우 else if또는 switch로 추가)
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


    // 오브젝트와 상호작용
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
                    // 실행
                    break;
                default:
                    break;
            }
        }
    }

    private void StartTalk()
    {
        UIManager UIManager = UIManager._Instance;
        // 대화중일때 `F`를 누르면 다음 대화로 넘어가기
        if (UIManager._DialogUI.gameObject.activeSelf)
        {
            UIManager._DialogUI.UpdateDialog();
            return;
        }

        QuestGiver giver = _hit.collider.GetComponent<QuestGiver>();
        #region 확장기능
        if (giver._CurrentQuest == null || giver._CurrentQuest._questState == QuestState.Unvaliable)
        {
            // 필요시 퀘스트가 없어도 가능한 Default 대화 추가
            return;
        }
        else if (giver._CurrentQuest._questState == QuestState.Accepted)
        {
            // 필요시 퀘스트 진행중일 경우의 대화 추가
            return;
        }
        #endregion

        StartCoroutine(CLookAtPlayer(giver));   // NPC 회전

        // 퀘스트 수동 완료
        if (giver._CurrentQuest._questState == QuestState.Completed)
        {
            int tempCode = giver._CurrentQuest._questCode;      // 다음 퀘스트로 넘어가기전 코드 저장
            giver._CurrentQuest.Done();
            UIManager._QuestListUI.UpdateList(tempCode);
            return;
        }
        
        UIManager._DialogUI.OpenDialog();
    }

    private IEnumerator CLookAtPlayer(QuestGiver giver)
    {
        float rotateTime = 0f;
        Vector3 dir = this.gameObject.transform.position - giver.transform.position;
        Quaternion q = Quaternion.LookRotation(dir);
        while(1 > rotateTime)
        {
            giver.transform.rotation = Quaternion.Slerp(giver.transform.rotation, q, Time.deltaTime * _rotateSpeed);
            rotateTime += Time.deltaTime * _rotateSpeed;
            yield return null;
        }    
    }
}

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
    [SerializeField] private CharacterController _charac;
    [SerializeField] private RaycastHit _hit;
    [SerializeField] private LayerMask _mask;

    [SerializeField] private QuestUI _questUI;
    [SerializeField] private InteractionObjectType _interObj;
    [SerializeField] private bool _canInterect;

    private int _npc;

    public RaycastHit _Hit => _hit;

    private void Awake()
    {
        _charac = GetComponent<CharacterController>();

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
        Debug.DrawRay(_charac.transform.position + Vector3.up * 0.5f, transform.forward, Color.blue, 1f);
        if (Physics.Raycast(_charac.transform.position + Vector3.up * 0.5f, transform.forward, out _hit, 1f, _mask))
        {
            _canInterect = true;

            if (_hit.collider.gameObject.layer == _npc)
            {
                _interObj = InteractionObjectType.NPC;
            }
        }
        else
        {
            _canInterect = false;
        }
    }


    // 오브젝트와 상호작용
    private void InteractObject()
    {
        if (_canInterect && Input.GetKeyDown(KeyCode.F1))
        {
            switch (_interObj)
            {
                case InteractionObjectType.NPC:
                    StartTalk();
                    break;
                case InteractionObjectType.Item:
                    // 실행
                    break;
                default:
                    Debug.Log("Somthing wrong value -> " + _interObj);
                    break;
            }
        }
    }

    private void StartTalk()
    {
        UIManager._Instance._DialogUI.OpenDialog();
    }
}

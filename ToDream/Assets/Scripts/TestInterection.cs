using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterectionObjectType
{
    Default,
    NPC,
    Item
}

public class TestInterection : MonoBehaviour
{
    [SerializeField] CharacterController _charac;
    [SerializeField] RaycastHit _hit;
    [SerializeField] LayerMask _mask;

    [SerializeField] QuestUI _questUI;
    [SerializeField] InterectionObjectType _interObj;
    [SerializeField] bool _canInterect;

    int _npc;


    private void Awake()
    {
        _charac = GetComponent<CharacterController>();

        _npc = LayerMask.NameToLayer("NPC");
    }

    private void Update()
    {
        OnInterection();
        InterectObject();
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
                _interObj = InterectionObjectType.NPC;
            }
        }
        else
        {
            _canInterect = false;
        }
    }


    // 오브젝트와 상호작용
    private void InterectObject()
    {
        if (_canInterect && Input.GetKeyDown(KeyCode.F1))
        {
            switch (_interObj)
            {
                case InterectionObjectType.NPC:
                    OpenQuestUI();
                    break;
                case InterectionObjectType.Item:
                    // 실행
                    break;
                default:
                    Debug.Log("Somthing wrong value -> " + _interObj);
                    break;
            }
        }
    }

    public void OpenQuestUI()
    {
        Quest quest = _hit.collider.GetComponent<QuestGiver>()._CurrentQuest;
        
        if (quest._QuestState != QuestState.Avaliable) { return; }

        _questUI.gameObject.SetActive(true);
        _questUI.SetText(quest._Title, quest._Description);
    }

    public void CloseQuestUI()
    {
        _questUI.gameObject.SetActive(false);
    }

    public void AcceptedQuest()
    {
        Quest quest = _hit.collider.GetComponent<QuestGiver>()._CurrentQuest;
        quest.Accepted();
        QuestManager._Instance._acceptedQuests.Add(quest);
        Debug.Log("Accept Quest -> " + quest);
        CloseQuestUI();
    }
}

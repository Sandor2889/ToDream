using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestNavigation : MonoBehaviour
{
    [SerializeField] private float _searchDistance = 10f;
    [SerializeField] private GameObject _navArrow;
    private Transform[] _targetTr;

    private void Awake()
    {
        _targetTr = new Transform[3];
    }

    public void OnEnable()
    {
        // ������ ����Ʈ�� ������ return
        if(!CheckHasQuest()) { return; }

        Quest currQuest = QuestManager._Instance._acceptedQuests[0];
        if (currQuest._questState == QuestState.Accepted)   // ����Ʈ �������ΰ�� Nav�� ��ǥ������ ����Ų��.
        {
            List<QuestGoal> copyList = currQuest._questGoals.ToList<QuestGoal>();
            for (int i = 0; i < _targetTr.Length; i++)
            {
                for (int j = 0; j < copyList.Count; j++)
                {
                    // �̹� �޼��� ��ǥ�� ���ܽ�Ű�� 
                    if (!copyList[j]._targetMarker.gameObject.activeSelf)
                    {
                        copyList.Remove(copyList[j]);
                    }

                    _targetTr[i] = copyList[j]._targetMarker.transform;
                    copyList.Remove(copyList[j]);
                    break;
                }
            }
        }
        else if (currQuest._questState == QuestState.Completed) // ����Ʈ �Ϸᰡ�� ���¸� NPC�� ����Ų��.
        {
            _targetTr[0] = UIManager._Instance._NPCMarkerUI._Npcs[(int)currQuest._npcName].transform;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _targetTr.Length; i++)
        {
            _targetTr[i] = null;
        }
    }


    private void Update()
    {
        Vector3 offset = _targetTr[0].position - transform.position;
        float sqrDist = offset.sqrMagnitude;

        // Ž������������ ������ return
        if (sqrDist < _searchDistance * _searchDistance) 
        {
            _navArrow.GetComponent<MeshRenderer>().enabled = false;
            return;
        }

        // �������� �����ִٸ� ���ֱ�
        _navArrow.GetComponent<MeshRenderer>().enabled = true;
        transform.LookAt(_targetTr[0]);
    }

    public bool CheckHasQuest()
    {
        if (QuestManager._Instance._acceptedQuests.Count <= 0) 
        { 
            return false; 
        }
        return true;
    }

    public void OnNav()
    {
        if (!CheckHasQuest()) { return; }

        gameObject.SetActive(true);
    }

    public void OffNav()
    {
        gameObject.SetActive(false);
    }
}

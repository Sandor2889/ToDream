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
        _targetTr = new Transform[3];   // 한 번에 수용가능한 수 -> QuestGoal (3개이상은 잘 없다)
    }

    public void OnEnable()
    {
        Quest currQuest = QuestManager._Instance._acceptedQuests[0];    // 현재 수락한 퀘스트 중 가장 먼저 수락한 퀘스트
        if (currQuest._questState == QuestState.Accepted)   // 퀘스트 진행중인경우 Nav가 목표지점을 가리킨다.
        {
            List<QuestGoal> copyList = currQuest._questGoals.ToList<QuestGoal>();   // 퀘스트의 목표들을 복사
            for (int i = 0; i < _targetTr.Length; i++)
            {
                for (int j = 0; j < copyList.Count; j++)
                {
                    // 이미 달성한 목표는 제외시키기 
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
        else if (currQuest._questState == QuestState.Completed) // 퀘스트 완료가능 상태면 NPC를 가리킨다.
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

        // 탐지범위안으로 들어오면 nav 끄기
        if (sqrDist < _searchDistance * _searchDistance) 
        {
            if (_navArrow.GetComponent<MeshRenderer>().enabled)
            {
                _navArrow.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else 
        {
            // 렌더러가 꺼져있다면 켜주기
            if (!_navArrow.GetComponent<MeshRenderer>().enabled)
            {
                _navArrow.GetComponent<MeshRenderer>().enabled = true;
            }

            transform.LookAt(_targetTr[0]);
        }
    }

    public void OnNav()
    {
        if (!QuestManager.CheckHasQuest()) { return; }

        gameObject.SetActive(true);
    }

    public void OffNav()
    {
        gameObject.SetActive(false);
    }
}

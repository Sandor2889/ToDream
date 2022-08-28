using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestNavigation : MonoBehaviour
{
    [SerializeField] private float _searchDistance = 10f;
    private Transform[] _targetTr;
    private MeshRenderer[] _navRenderer;

    private void Awake()
    {
        _targetTr = new Transform[3];
        _navRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    public void OnEnable()
    {
        // 수락한 퀘스트가 없으면 return
        if(!CheckHasQuest()) { return; }

        Quest currQuest = QuestManager._Instance._acceptedQuests[0];
        List<QuestGoal> copyList = currQuest._questGoals.ToList<QuestGoal>();

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

        // 탐지범위안으로 들어오면 return
        if (sqrDist < _searchDistance * _searchDistance) 
        {
            for (int i = 0; i < _navRenderer.Length; i++)
            {
                if (_navRenderer[i].enabled)
                {
                    _navRenderer[i].enabled = false;
                }
            }
            return;
        }

        // 렌더러가 꺼져있다면 켜주기
        for (int i = 0; i < _navRenderer.Length; i++)
        {
            if (!_navRenderer[i].enabled)
            {
                _navRenderer[i].enabled = true;
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    private static QuestManager _instance;
    public static QuestManager _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("QuestManager").AddComponent<QuestManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private ParticleSystem _completeEffect;

    public List<Quest> _quests = new List<Quest>();             // 퀘스트 데이터베이스
    public List<Quest> _acceptedQuests = new List<Quest>();     // 수락한 퀘스트
    public List<Quest> _doneQuests = new List<Quest>();         // 완료된 퀘스트

    public ParticleSystem _CompleteEffect => _completeEffect;

    private void Awake()
    {
        _instance = this;
        InitCode();
    }

    // 퀘스트 고유번호 초기화
    private void InitCode()
    {
        for (int i = 0; i < _quests.Count; i++)
        {
            _quests[i].InitCode();
        }
    }

    // 퀘스트 진행률 업데이트
    public void ReceiveReport(string target, int counting)
    {
        for (int i = 0; i < _acceptedQuests.Count; i++)
        {
            if (!IsTarget(_acceptedQuests[i])) { continue; }

            _acceptedQuests[i].ReceiveReport(target, counting);
        }
    }

    // 퀘스트의 Target과 State 조건 확인
    public bool IsTarget(Quest quest)
    {
        if (quest._questState == QuestState.Accepted )
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 완료된 퀘스트가 자신의 Code를 Manager로 전송시키고
    /// 이 함수를 통해 선행 조건을 가진 Quest에 접근하여 Code를 비교하여 찾아낸다.
    /// </summary>
    /// <param name="doneCode"></param>
    public void FindQuestAsCode(int doneCode)
    {
        for (int i = 0; i < _quests.Count; i++)
        {
            if (_quests[i]._questState == QuestState.Unvaliable && _quests[i]._hasConditions)
            {
                for (int j = 0; j < _quests[i]._conditions.Count; j++)
                {
                    _quests[i]._conditions[j].UpdateConditions(doneCode);
                    _quests[i].CompleteAllCondition();
                }
            }
        }
    }

    // 수락한 퀘스트가 있는지 검사
    public static bool CheckHasQuest()
    {
        if (_instance._acceptedQuests.Count <= 0)
        {
            return false;
        }
        return true;
    }
}

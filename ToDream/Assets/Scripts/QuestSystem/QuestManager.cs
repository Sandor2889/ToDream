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

    public List<Quest> _quests = new List<Quest>();             // ����Ʈ �����ͺ��̽�
    public List<Quest> _acceptedQuests = new List<Quest>();     // ������ ����Ʈ
    public List<Quest> _doneQuests = new List<Quest>();         // �Ϸ�� ����Ʈ

    public ParticleSystem _CompleteEffect => _completeEffect;

    private void Awake()
    {
        _instance = this;
        InitCode();
    }

    // ����Ʈ ������ȣ �ʱ�ȭ
    private void InitCode()
    {
        for (int i = 0; i < _quests.Count; i++)
        {
            _quests[i].InitCode();
        }
    }

    // ����Ʈ ����� ������Ʈ
    public void ReceiveReport(string target, int counting)
    {
        for (int i = 0; i < _acceptedQuests.Count; i++)
        {
            if (!IsTarget(_acceptedQuests[i])) { continue; }

            _acceptedQuests[i].ReceiveReport(target, counting);
        }
    }

    // ����Ʈ�� Target�� State ���� Ȯ��
    public bool IsTarget(Quest quest)
    {
        if (quest._questState == QuestState.Accepted )
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// �Ϸ�� ����Ʈ�� �ڽ��� Code�� Manager�� ���۽�Ű��
    /// �� �Լ��� ���� ���� ������ ���� Quest�� �����Ͽ� Code�� ���Ͽ� ã�Ƴ���.
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

    // ������ ����Ʈ�� �ִ��� �˻�
    public static bool CheckHasQuest()
    {
        if (_instance._acceptedQuests.Count <= 0)
        {
            return false;
        }
        return true;
    }
}

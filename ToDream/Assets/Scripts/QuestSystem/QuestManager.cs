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

    public List<Quest> _questContainer = new List<Quest>();     // ����Ʈ �����ͺ��̽�
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
        for (int i = 0; i < _questContainer.Count; i++)
        {
            _questContainer[i].InitCode();
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

    // ����Ʈ�� State ���� Ȯ��
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
        for (int i = 0; i < _questContainer.Count; i++)
        {
            if (_questContainer[i]._questState == QuestState.Unvaliable && _questContainer[i]._hasConditions)
            {
                for (int j = 0; j < _questContainer[i]._conditions.Count; j++)
                {
                    _questContainer[i]._conditions[j].UpdateConditions(doneCode);
                    _questContainer[i].CompleteAllCondition();
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

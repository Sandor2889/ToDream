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

    public List<Quest> _quests = new List<Quest>();             // 퀘스트 데이터베이스
    public List<Quest> _acceptedQuests = new List<Quest>();    // 수락한 퀘스트
    public List<Quest> _doneQuests = new List<Quest>();    // 완료된 퀘스트

    private void Awake()
    {
        _instance = this;
        InitQuestState();
    }

    private void InitQuestState()
    {
        for (int i = 0; i < _quests.Count; i++)
        {
            _quests[i].Avaliable();
        }
    }

    public void ReceiveReport(string target, int counting)
    {
        for(int i = 0; i < _acceptedQuests.Count; i++)
        {
            _acceptedQuests[i].ReceiveReport(target, counting);
        }
    }
}

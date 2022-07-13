using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    #region Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion

    #region Singleton
    private static QuestSystem _instance;
    private static bool _isApplicationQuitting;

    public static QuestSystem _Instance
    {
        get
        {
            if (!_isApplicationQuitting && _instance == null)
            {
                _instance = FindObjectOfType<QuestSystem>();
                if (_instance == null)
                {
                    _instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }
    #endregion

    private List<Quest> _activeQuests = new List<Quest>();
    private List<Quest> _completedQuests = new List<Quest>();

    private QuestDataBase _questDatabase;

    public event QuestRegisteredHandler _onQuestRegistered;
    public event QuestCompletedHandler _onQuestCompleted;
    public event QuestCanceledHandler _onQuestCanceled;

    public IReadOnlyList<Quest> _ActiveQuests => _activeQuests;
    public IReadOnlyList<Quest> _CompletedQuests => _completedQuests;

    private void Awake()
    {
        _questDatabase = Resources.Load<QuestDataBase>("QuestDatabase");
    }

    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if (newQuest is Quest)
        {
            newQuest._onCompleted += OnQuestCompleted;
            newQuest._onCanceled += OnQuestCanceled;

            _activeQuests.Add(newQuest);
            newQuest.OnRegister();
            _onQuestRegistered?.Invoke(newQuest);
        }
        return newQuest;
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(_activeQuests, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category._ID, target._Value, successCount);

    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        foreach (var quest in quests.ToArray())
        {
            quest.ReceiveReport(category, target, successCount);
        }
    }

    public bool ContainsInActiveQuests(Quest quest) => _activeQuests.Any(x => x._ID == quest._ID);
    public bool ContainsInCompleteQuests(Quest quest) => _completedQuests.Any(x => x._ID == quest._ID);

    #region Callback
    /// <summary>
    ///  Quest event에 등록하면 System에서 확인을 해주지 않아도
    ///  알아서 목록에서 추가 및 제거를 한다.
    /// </summary>
    /// <param name="quest"></param>
    private void OnQuestCompleted(Quest quest)
    {
        _activeQuests.Remove(quest);
        _completedQuests.Add(quest);
        _onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        _activeQuests.Remove(quest);
        _onQuestCanceled?.Invoke(quest);
        Destroy(quest, Time.deltaTime);
    }
    #endregion
}

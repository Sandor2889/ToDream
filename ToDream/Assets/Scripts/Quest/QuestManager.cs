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
                return null;
            }
            else
            {
                return _instance;
            }
        }
    }
    #endregion

    public List<Quest> _questList = new List<Quest>();
    public List<Quest> _currentQuestList = new List<Quest>();

    private void Awake()
    {
        _instance = this;
    }

    // 퀘스트 수락
    public void AcceptQuest(int questId)
    {
        for (int idx = 0; idx < _questList.Count; idx++)
        {
            if (_questList[idx]._id == questId && _questList[idx]._progress == Quest.QuestProgress.Availavle)
            {
                _currentQuestList.Add(_questList[idx]);
                _questList[idx]._progress = Quest.QuestProgress.Accepted;
            }
        }
    }

    // 퀘스트 진행 - 퀘스트 아이템 갱신 및 완료 조건
    public void AddQuestItem(string questObj, int itemAmount)
    {
        for (int idx = 0; idx < _currentQuestList.Count; idx++)
        {
            // 아이템 갱신
            if (_currentQuestList[idx]._objective == questObj && _currentQuestList[idx]._progress == Quest.QuestProgress.Accepted)
            {
                _currentQuestList[idx]._objectiveCount += itemAmount;
            }

            // 퀘스트 완료 조건
            if (_currentQuestList[idx]._objectiveCount >= _currentQuestList[idx]._objectiveRequirement && _currentQuestList[idx]._progress == Quest.QuestProgress.Accepted)
            {
                _currentQuestList[idx]._progress = Quest.QuestProgress.Complete;
            }
        }
    }

    // 퀘스트 완료
    public void CompletedQuest(int questId)
    {
        for (int idx = 0; idx < _currentQuestList.Count; idx++)
        {
            if (_currentQuestList[idx]._id == questId && _currentQuestList[idx]._progress == Quest.QuestProgress.Accepted)
            {
                _currentQuestList[idx]._progress = Quest.QuestProgress.Done;
                _currentQuestList.Remove(_currentQuestList[idx]);
            }
        }
    }

    // 퀘스트 포기
    public void GiveUpQuest(int questId)
    {
        for (int idx = 0; idx < _currentQuestList.Count; idx++)
        {
            if (_currentQuestList[idx]._id == questId && _currentQuestList[idx]._progress == Quest.QuestProgress.Accepted)
            {
                _currentQuestList[idx]._progress = Quest.QuestProgress.Availavle;
                _currentQuestList[idx]._objectiveCount = 0;
                _currentQuestList.Remove(_currentQuestList[idx]);
            }
        }
    }

    public bool RequestAvailableQuest(int questId)
    {
        for (int idx = 0; idx < _questList.Count; idx++)
        {
            if(_questList[idx]._id == questId && _questList[idx]._progress == Quest.QuestProgress.Availavle)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptQuest(int questId)
    {
        for (int idx = 0; idx < _questList.Count; idx++)
        {
            if (_questList[idx]._id == questId && _questList[idx]._progress == Quest.QuestProgress.Accepted)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompletedQuest(int questId)
    {
        for (int idx = 0; idx < _questList.Count; idx++)
        {
            if (_questList[idx]._id == questId && _questList[idx]._progress == Quest.QuestProgress.Complete)
            {
                return true;
            }
        }
        return false;
    }
}

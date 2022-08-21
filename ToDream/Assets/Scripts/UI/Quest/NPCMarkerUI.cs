using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMarkerUI : MonoBehaviour
{
    [SerializeField] private Image[] _baseImages;

    private Sprite _avaliableMarker;
    [SerializeField] private Sprite _inProgressMarker;
    private Sprite _completedMarker;

    private void Awake()
    {
        InitMarkerSprites();
    }

    public void InitMarkerSprites()
    {
        _avaliableMarker = Resources.Load<Sprite>("NPCMarker/Avaliable");
        //_inProgressMarker = Resources.Load<Sprite>("NPCMarker/InProgress");
        _completedMarker = Resources.Load<Sprite>("NPCMarker/Completed");
    }

    public void SettingByQuestState(int idx, QuestState questState)
    {
        switch (questState)
        {
            case QuestState.Unvaliable:
            case QuestState.Done:
                OffMarker(idx);
                break;
            case QuestState.Avaliable:
                OnMarker(idx);
                SetMarker(idx, _avaliableMarker);
                break;
            case QuestState.Accepted:
                SetMarker(idx, _inProgressMarker);
                break;
            case QuestState.Completed:
                SetMarker(idx, _completedMarker);
                break;
            default:
                break;
        }
    }


    public void OnMarker(int idx)
    {
        _baseImages[idx].gameObject.SetActive(true);
    }

    public void OffMarker(int idx)
    {
        _baseImages[idx].gameObject.SetActive(false);
    }

    public void SetMarker(int idx, Sprite marker)
    {
        _baseImages[idx].sprite = marker;
    }
}

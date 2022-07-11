using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum QuestProgress
    {
        UnAvailavle,
        Availavle,
        Accepted,
        Complete,
        Done
    }

    public string _title;            // Quest 제목
    public int _id;                  // Quest 고유 번호
    public string _desc;             // Quest 내용
    public int _nextQuest;           // 연계 Quest 
    public QuestProgress _progress;  // Quest 현재 상태

    public string _objective;   // Quest 목표
    public int _objectiveRequirement; // Quest 목표 수량
    public int _objectiveCount;       // Quest 목펴 현재 수량

    public int _expReward;
    public int _goldReward;
    // 필요시 아이템보상 추가
}

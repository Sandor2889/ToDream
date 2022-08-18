using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButtonPool : MonoBehaviour
{
    [SerializeField] private GameObject _buttonObj;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int _count;

    public Queue<QButtenInList> _qButtons = new Queue<QButtenInList>();

    private void Awake()
    {
        InitObj(_count);
    }

    private QButtenInList CreateNewObj()
    {
        QButtenInList obj = Instantiate(_buttonObj).GetComponent<QButtenInList>();
        obj.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    private void InitObj(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _qButtons.Enqueue(CreateNewObj());     
        }
    }

    public QButtenInList GetObj(Quest quest)
    {
        if (_qButtons.Count > 0)
        {
            QButtenInList obj = _qButtons.Dequeue();
            obj._quest = quest;
            obj.SetText();
            obj.transform.SetParent(_scrollRect.content);
            obj.gameObject.SetActive(true);

            UIManager._Instance._QuestListUI._qButtons.Add(obj);
            return obj;
        }
        else
        {
            // 퀘스트 수락 가능 한도제한 팝업 실행하기
            Debug.Log("MAX");
            return null;
        }
    }

    public void ReturnObj(QButtenInList qButton)
    {
        qButton._quest = null;
        qButton.transform.SetParent(transform);
        qButton.gameObject.SetActive(false);
        _qButtons.Enqueue(qButton);

        UIManager._Instance._QuestListUI._qButtons.Remove(qButton);
    }
}

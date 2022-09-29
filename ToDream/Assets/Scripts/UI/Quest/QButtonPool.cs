using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButtonPool : MonoBehaviour
{
    [SerializeField] private GameObject _buttonObj;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int _count;

    public Queue<QButtonInList> _qButtons = new Queue<QButtonInList>();

    private void Awake()
    {
        InitObj(_count);
    }

    private QButtonInList CreateNewObj()
    {
        QButtonInList obj = Instantiate(_buttonObj).GetComponent<QButtonInList>();
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

    // ����Ʈ ������ ȣ��
    public QButtonInList GetObj(Quest quest)
    {
        if (_qButtons.Count > 0)    // ���� ������ ���ڸ��� �ִٸ� ����
        {
            QButtonInList obj = _qButtons.Dequeue();
            obj._quest = quest;
            obj.transform.SetParent(_scrollRect.content);
            obj.gameObject.SetActive(true);

            UIManager._Instance._QuestListUI._qButtons.Add(obj);
            return obj;
        }
        else
        {
            // ����Ʈ ���� ���� �ѵ����� �˾� �����ϱ�
            return null;
        }
    }

    public void ReturnObj(QButtonInList qButton)
    {
        qButton._quest = null;
        qButton.transform.SetParent(transform);
        qButton.gameObject.SetActive(false);
        _qButtons.Enqueue(qButton);

        UIManager._Instance._QuestListUI._qButtons.Remove(qButton);
    }
}

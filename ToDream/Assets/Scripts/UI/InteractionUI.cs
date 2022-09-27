using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private string _talkText;  // 인스펙터에서 텍스트 입력
    private string _itemText;

    public Text _text;

    public void OnInterText()
    {
        gameObject.SetActive(true);
    }

    public void OffInterText()
    {
        gameObject.SetActive(false);
    }

    public void SetText(InteractionObjectType interObj)
    {
        switch (interObj)
        {
            case InteractionObjectType.NPC:
                _text.text = _talkText;
                break;
            case InteractionObjectType.Item:
                _text.text = _itemText;
                break;
            default:
                break;
        }
    }
}

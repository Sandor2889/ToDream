using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _itemBackground;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _stateText;
    [SerializeField] private VehicleRegistration _registration;
    [SerializeField] private Vector3 _offset;
    
    public int _itemKey;

    public void Awake()
    {
        _registration = FindObjectOfType<VehicleRegistration>(true);
    }

    public void SetItem(int itemKey)
    {
        Item item = GameManager.GetDicValue(itemKey);
        _itemKey = item._key;
        _itemImage.sprite = item._sprite;
        SetAlpha(1);
    }

    public void Clear()
    {
        _itemImage.sprite = null;
        _itemKey = -1;
        SetAlpha(0);
    }

    public void SetAlpha(int alpha)
    {
        Color color = _itemImage.color;
        color.a = alpha;
        _itemImage.color = color;

        Color backColor = _itemBackground.color;
        backColor.a = 145 / 255f * alpha;
        _itemBackground.color = backColor;
    }

    public void SetRegisteredText(GameObject itemObj)
    {
        if (_itemKey == -1)
        {
            _stateText.gameObject.SetActive(false);
        }
        else
        {
            if (itemObj == null || 
                !itemObj.GetComponent<RegistrationState>()._isRegistered || 
                _itemKey != itemObj.GetComponent<RegistrationState>()._itemKey)
            {
                _stateText.text = "OFF";
            }
            else if (itemObj.GetComponent<RegistrationState>()._isRegistered &&
                _itemKey == itemObj.GetComponent<RegistrationState>()._itemKey)
            {
                _stateText.text = "ON";
            }

            _stateText.gameObject.SetActive(true);
        }
    }


    // 슬롯 클릭시 탈것 등록 버튼 호출
    public void OnRegistrationButton()
    {
        if (_itemKey == -1) { return; }

        _registration._itemKey = _itemKey;
        _registration._buttonPanel.transform.position = gameObject.transform.position + _offset;
        _registration.gameObject.SetActive(true);
    }
}

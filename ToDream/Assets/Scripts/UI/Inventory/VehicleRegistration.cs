using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleRegistration : MonoBehaviour
{
    public Dictionary<int, GameObject> _registeredObjs = new Dictionary<int, GameObject>();  // 씬에 배치된 아이템

    public GameObject _buttonPanel;
    public int _itemKey;
    public Button _release;

    private void Awake()
    {
        VehicleWheelController._currentVehicles[0] = FindObjectOfType<CharacterController>(true).gameObject;
    }

    private void OnDisable()
    {
        SetReleaseButtonInter(false);
    }

    // 등록
    public void OnRegister()
    {
        GameObject obj = GameManager.GetDicValue(_itemKey)._obj;
        int category = (int)GameManager.GetDicValue(_itemKey)._category;

        GameObject preObj = VehicleWheelController._currentVehicles[category + 1];

        if (!_registeredObjs.ContainsKey(_itemKey)) // 첫 등록 시
        {
            obj = Instantiate(obj);
            VehicleWheelController._currentVehicles[category + 1] = obj;
            _registeredObjs.Add(_itemKey, obj);
            obj.SetActive(false);
        }
        else
        {
            _registeredObjs.TryGetValue(_itemKey, out obj);
            VehicleWheelController._currentVehicles[category + 1] = obj;
        }

        // slot의 ON/OFF Text - 현재 등록된 카테고리의 탈것과 다른것을 등록할 경우
        if (preObj != null && preObj != obj)
        {
            preObj.GetComponent<RegistrationState>()._isRegistered = false;
        }
        obj.GetComponent<RegistrationState>()._isRegistered = true;
        UIManager._Instance._InventoryUI.UpdateRegisteredSlot();
        OnBack();
    }

    // 등록 해제
    public void Release()
    {
        Item item = GameManager.GetDicValue(_itemKey);
        int category = (int)item._category;

        GetObjDicValue().GetComponent<RegistrationState>()._isRegistered = false;
        VehicleWheelController._currentVehicles[category + 1] = null;
        UIManager._Instance._InventoryUI.UpdateRegisteredSlot();
        OnBack();
    }

    public void OnBack()
    {
        this.gameObject.SetActive(false);
    }

    public GameObject GetObjDicValue()
    {
        GameObject obj;
        _registeredObjs.TryGetValue(_itemKey, out obj);
        return obj;
    }

    // 해제 버튼 상호작용 On/Off
    public void SetReleaseButtonInter(bool enable)
    {
        _release.interactable = enable;     
    }
}

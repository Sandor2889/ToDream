using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleRegistration : MonoBehaviour
{
    public Dictionary<int, GameObject> _registeredObjs = new Dictionary<int, GameObject>();  // ���� ��ġ�� ������

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

    // ���
    public void OnRegister()
    {
        GameObject obj = GameManager.GetDicValue(_itemKey)._obj;
        int category = (int)GameManager.GetDicValue(_itemKey)._category;

        GameObject preObj = VehicleWheelController._currentVehicles[category + 1];

        if (!_registeredObjs.ContainsKey(_itemKey)) // ù ��� ��
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

        // slot�� ON/OFF Text - ���� ��ϵ� ī�װ��� Ż�Ͱ� �ٸ����� ����� ���
        if (preObj != null && preObj != obj)
        {
            preObj.GetComponent<RegistrationState>()._isRegistered = false;
        }
        obj.GetComponent<RegistrationState>()._isRegistered = true;
        UIManager._Instance._InventoryUI.UpdateRegisteredSlot();
        OnBack();
    }

    // ��� ����
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

    // ���� ��ư ��ȣ�ۿ� On/Off
    public void SetReleaseButtonInter(bool enable)
    {
        _release.interactable = enable;     
    }
}

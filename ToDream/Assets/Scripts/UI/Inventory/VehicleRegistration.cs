using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleRegistration : MonoBehaviour
{
    private Dictionary<int, GameObject> _registeredObjs = new Dictionary<int, GameObject>();        // ���� ��ġ�� ������

    public GameObject _buttonPanel;
    public int _itemKey;

    private void Awake()
    {
        VehicleWheelController._currentVehicles[0] = FindObjectOfType<CharacterController>(true).gameObject;
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

        // slot�� stateText - ���� ��ϵ� ī�װ��� Ż�Ͱ� �ٸ����� ����� ���
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
        int category = (int)GameManager.GetDicValue(_itemKey)._category;

        VehicleWheelController._currentVehicles[category + 1] = null;
        UIManager._Instance._InventoryUI.UpdateRegisteredSlot();
        OnBack();
    }

    public void OnBack()
    {
        this.gameObject.SetActive(false);
    }
}

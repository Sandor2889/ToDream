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
        // �⺻ �� �ʱ�ȭ - ĳ���� ���
        //_registeredObjs[0] = 
    }

    public void OnRegister()
    {
        GameObject obj = GameManager.GetDicValue(_itemKey)._obj;
        int category = (int)GameManager.GetDicValue(_itemKey)._category;

        if (!_registeredObjs.ContainsKey(_itemKey))
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
        
        OnBack();
    }

    public void OnBack()
    {
        this.gameObject.SetActive(false);
    }
}

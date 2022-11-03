using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyObject : MonoBehaviour
{
    private Rigidbody _rigid;
    //private int // ���� üũ
    [SerializeField] private bool _isUnderwater;
    [SerializeField] private int _floatersUnderwater;

    [SerializeField] private Transform[] _floaters;

    // ��ü(����) ����
    [SerializeField] private float _underWaterDrag = 3f;
    [SerializeField] private float _underWaterAngularDrag = 1f;

    // ���� ����
    [SerializeField] private float _airDrag = 0f;
    [SerializeField] private float _airAngularDrag = 0.05f;
 
    [SerializeField] private float _floatingForce = 15f;    // �η�
    [SerializeField] private float _waterHeight = -6.05f;   // �� ����
    [SerializeField] private float _waveSize = 0.5f;        // �ĵ� ũ��
    // Sin ũ��
    [SerializeField] private float frequency = 1f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _floatersUnderwater = 0;
        float waveHeight = _waveSize * Mathf.Sin(Time.time) + _waterHeight;
        for (int i = 0; i < _floaters.Length; i++)
        {
            float difference = _floaters[i].position.y - waveHeight;  // ��ü�� �� ������ �� (�� ������ �� �������� ��Ÿ��)
            if (difference < 0) // ���� 0 ���� �Ʒ��� ����
            {
                _rigid.AddForceAtPosition(Vector3.up * _floatingForce * Mathf.Abs(difference), _floaters[i].position, ForceMode.Force);
                _floatersUnderwater += 1;
                if (!_isUnderwater)
                {
                    _isUnderwater = true;
                    SwitchState(true);
                }
            }
        }

        if (_isUnderwater && _floatersUnderwater == 0)
        {
            _isUnderwater = false;
            SwitchState(false);
        }
    }

    // drag �� ���� (���� <-> ����)
    private void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            _rigid.drag = _underWaterDrag;
            _rigid.angularDrag = _underWaterAngularDrag;
        }
        else
        {
            _rigid.drag = _airDrag;
            _rigid.angularDrag = _airAngularDrag;
        }
    }
}

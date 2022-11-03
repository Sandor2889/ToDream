using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyObject : MonoBehaviour
{
    private Rigidbody _rigid;
    //private int // 물속 체크
    [SerializeField] private bool _isUnderwater;
    [SerializeField] private int _floatersUnderwater;

    [SerializeField] private Transform[] _floaters;

    // 유체(물속) 저항
    [SerializeField] private float _underWaterDrag = 3f;
    [SerializeField] private float _underWaterAngularDrag = 1f;

    // 공기 저항
    [SerializeField] private float _airDrag = 0f;
    [SerializeField] private float _airAngularDrag = 0.05f;
 
    [SerializeField] private float _floatingForce = 15f;    // 부력
    [SerializeField] private float _waterHeight = -6.05f;   // 물 높이
    [SerializeField] private float _waveSize = 0.5f;        // 파도 크기
    // Sin 크기
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
            float difference = _floaters[i].position.y - waveHeight;  // 물체와 물 높이의 차 (물 속인지 물 밖인지를 나타냄)
            if (difference < 0) // 차가 0 보다 아래면 물속
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

    // drag 값 변경 (물속 <-> 물밖)
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

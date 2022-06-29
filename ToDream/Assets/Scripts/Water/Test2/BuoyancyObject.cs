using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{ 
    public float _underwaterDrag = 3f;          // 물속 저항력
    public float _underwaterAngularDrag = 1f;   // 물속 회전 저항력
    public float _floatingForce = 50f;          // 부력
    private float _waterHeight = 0f;             // 파도의 높이

    private float _normalDrag = 0f;                 // 기본 저항력
    private float _normalAngularDrag = 0.05f;       // 기본 회전 저항력
    
    [SerializeField] private Transform[] _floaters; 
    private int _floatersUnderwater;
    private bool _underwater;

    private Rigidbody _rigid;
    private OceanManager _oceanMgr;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _oceanMgr = FindObjectOfType<OceanManager>();

        // 기본 값 초기화
        _normalDrag = _rigid.drag;
        _normalAngularDrag = _rigid.angularDrag;
    }

    private void FixedUpdate()
    {
        _floatersUnderwater = 0;
        for (int i = 0; i < _floaters.Length; i++)
        {
            //float difference = _floaters[i].position.y - _oceanMgr.WaterHeightAtPosition(_floaters[i].position);
            float difference = _floaters[i].position.y - _waterHeight;

            if (difference < 0)
            {
                _rigid.AddForceAtPosition(Vector3.up * _floatingForce * Mathf.Abs(difference), _floaters[i].position, ForceMode.Force);
                _floatersUnderwater++;
                if (!_underwater)
                {
                    _underwater = true;
                    SwitchState(true);
                }         
            }
        }

        if (_underwater && _floatersUnderwater == 0)
        {
            _underwater = false;
            SwitchState(false);
        }

    }

    private void SwitchState(bool underwater)
    {
        if (underwater)
        {
            _rigid.drag = _underwaterDrag;
            _rigid.angularDrag = _underwaterAngularDrag;
        }
        else
        {
            _rigid.drag = _normalDrag;
            _rigid.angularDrag = _normalAngularDrag;
        }
    }
}

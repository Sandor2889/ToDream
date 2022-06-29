using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 객체의 rigid 설정 기준값  mass : 1, drag : 1
public class Floater : MonoBehaviour
{
    public Rigidbody _rigid;
    public float _depthBeforeSubmertged = 1f;   // 물에 잠기기전의 깊이
    public float _displacementAmount = 3f;  //  변위량
    public float _waterDrag = 0.99f; // 물 저항력
    public float _waterAngularDrag = 0.5f; // 물 속 회전 저항력

    private void Awake()
    {
        _rigid = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.GetWaveHeight(transform.position.x);
        
        if (transform.position.y < waveHeight)
        {
            // 부력
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / _depthBeforeSubmertged) * _displacementAmount;
            _rigid.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            
            // 물체가 물밖으로 튕기는 현상 제거
            _rigid.AddForce(displacementMultiplier * -_rigid.velocity * _waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            
            // 물체의 지나친 회전 억제
            _rigid.AddTorque(displacementMultiplier * -_rigid.angularVelocity * _waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCharacteristics : MonoBehaviour
{
	#region Constants

	private const float _mpsToMph = 2.23694f;

    #endregion
	
	private Rigidbody _rb;
	private FlightInput _input;
	private float _beginningDrag;
	private float _beginningAngularDrag;
	private float _maxMPS;
	private float _normalizedMPH;
	private float _flapDrag;
	
	[Header("Flight Character")]
	public FlightCharacteristic _flightCharacter;
	
	[Header("Characteristics")]
	public float _forwardSpeed;
	public float _mph;
	
	[Header("Lift")]
	public AnimationCurve _liftCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	
	[Header("Control")]
	public AnimationCurve _controlEfficiency = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	private float _controlSurfaceEfficiency;
	private float _pitchAngle;
	private float _rollAngle;
	
	[SerializeField] private float _angleOfAttack;
	
	public void InitCharacteristics(Rigidbody rb, FlightInput input)
	{
		_input = input;
		_rb = rb;
		_beginningDrag = _rb.drag;
		_beginningAngularDrag = _rb.angularDrag;
		_maxMPS = _flightCharacter._MaxMPS / _mpsToMph;
	}
	
	public void UpdateChracteristics()
	{
		if(_rb)
		{
			CalculateForwardSpeed();
			CalculateLift();
			CalculateDrag();
			
			HandleSurfaceEfficiency();
			HandlePitch();
			HandleYaw();
			HandleRoll();
			
			//HandleRigidbody();
		}
	}
	
	private void CalculateForwardSpeed()
	{
		// 월드 좌표계로 정의된 것을 로컬 좌표계로 변환
		Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
		_forwardSpeed = Mathf.Max(0f, localVelocity.z);
		_forwardSpeed = Mathf.Clamp(_forwardSpeed, 0f, _maxMPS);
		_mph = _forwardSpeed * _mpsToMph;
		
		// 현재 값이 최소, 최대 사이 몇 퍼센트 위치에 있는지 판단
		_normalizedMPH = Mathf.InverseLerp(0f, _flightCharacter._MaxMPS, _mph);
	}
	
	private void CalculateLift()
	{
		//calculate the angle of attack
		_angleOfAttack = Vector3.Dot(_rb.velocity.normalized, transform.forward);
		_angleOfAttack *= _angleOfAttack;
		//calculate and add lift
		Vector3 liftDirection = transform.up;
		
		// _normalizedMPH 값에 해당하는 위치의 그래프값을 읽어옴
		float liftPower = _liftCurve.Evaluate(_normalizedMPH) * _flightCharacter._MaxLiftPower;

		//add flap lift
		float finalLiftPower = _flightCharacter._FlapLiftPower * 1;
		//final lift
		Vector3 finalLiftForce = liftDirection * (liftPower + finalLiftPower) * _angleOfAttack;
		
		_rb.AddForce(finalLiftForce);
	}
	
	private void CalculateDrag()
	{
		//flap drag
		_flapDrag = Mathf.Lerp(_flapDrag, 1, .02f);
		//speed drag
		float speedDrag = _forwardSpeed * _flightCharacter._DragFactor;

		//sum of all drag forces
		// drag = 공기저항력 (0.001 : 단단한 금속 덩어리 ~ 10 : 깃털)
		// angular drag = 토크로 회전할 때 공기저항력
		float finalDrag = _beginningDrag + speedDrag + _flapDrag;
		_rb.drag = finalDrag;
		_rb.angularDrag = _beginningAngularDrag * _forwardSpeed;
	}
	
	private void HandlePitch() // mouse Y
	{
		//even though its called torque its a force that rotates rb
		Vector3 pitchTorque = _input._Pitch * _flightCharacter._PitchSpeed * transform.right * _controlSurfaceEfficiency;

		_rb.AddTorque(pitchTorque);
	}
	
	private void HandleYaw() // a , d
	{
		Vector3 yawTorque = _input._Yaw * _flightCharacter._YawSpeed * transform.up * _controlSurfaceEfficiency;
		_rb.AddTorque(yawTorque);
	}
	
	private void HandleRoll() // mouse X
	{
		Vector3 rightDir = transform.right;
		rightDir.y = 0;
		rightDir = rightDir.normalized;
		_rollAngle = Vector3.Angle(transform.right, rightDir);

		Vector3 rollTorque = -_input._Roll * _flightCharacter._RollSpeed * transform.forward * _controlSurfaceEfficiency;

		_rb.AddTorque(rollTorque);
	}
	
	// 원심력
	private void HandleBanking()
	{
		
	}
	
	private void HandleRigidbody()
	{
		if (_rb.velocity.magnitude > 5f)
		{
			Vector3 rbVelocity = Vector3.Slerp(_rb.velocity,
				transform.forward * _forwardSpeed,
				_forwardSpeed * _angleOfAttack * Time.deltaTime);

			_rb.velocity = rbVelocity;

			Quaternion rbRotation = Quaternion.Slerp(_rb.rotation,
				Quaternion.LookRotation(_rb.velocity.normalized, transform.up),
				Time.deltaTime
			);

			_rb.MoveRotation(rbRotation);

		}
	}
	
	private void HandleSurfaceEfficiency()
	{
		// 속도에 따라 결정되는 값
		_controlSurfaceEfficiency = _controlEfficiency.Evaluate(_normalizedMPH);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCharacteristics : MonoBehaviour
{
	#region Constants

	private const float _mpsToMph = 2.23694f;

    #endregion
	
	private Rigidbody _rb;
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

	
	private float _angleOfAttack;
	
	public void InitCharacteristics(Rigidbody rb)
	{
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
			
			HandleRigidbody();
		}
	}
	
	private void CalculateForwardSpeed()
	{
		// 월드 좌표계로 정의된 것을 로컬 좌표계로 변환
		Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
		_forwardSpeed = Mathf.Max(0f, localVelocity.z);
		_forwardSpeed = Mathf.Clamp(_forwardSpeed, 0f, _maxMPS);
		_mph = _forwardSpeed * _mpsToMph;
		_normalizedMPH = Mathf.InverseLerp(0f, _flightCharacter._MaxMPS, _mph);
	}
	
	private void CalculateLift()
	{
		//calculate the angle of attack
		_angleOfAttack = Vector3.Dot(_rb.velocity.normalized, transform.forward);
		_angleOfAttack *= _angleOfAttack;
		//calculate and add lift
		Vector3 liftDirection = transform.up;
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
		float finalDrag = _beginningDrag + speedDrag + _flapDrag;
		_rb.drag = finalDrag;
		_rb.angularDrag = _beginningAngularDrag * _forwardSpeed;
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
}

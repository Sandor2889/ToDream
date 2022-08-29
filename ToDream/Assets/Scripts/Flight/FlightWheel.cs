using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class FlightWheel : MonoBehaviour
{
	[Header("Properties")]
	private WheelCollider _wheelCollider;
	private Vector3 _worldPos;
	private Quaternion _worldRot;
	public Transform _targetObj;
	
	public bool _IsGrounded { get{return _wheelCollider.isGrounded;} }
	
	private void Start()
	{
		_wheelCollider = GetComponent<WheelCollider>();
	}
	
	public void Init()
	{
		if(_wheelCollider)
		{
			_wheelCollider.motorTorque = .00000000000001f;
		}
	}
	
	public void HandleWheel()
	{
		if(_wheelCollider)
		{
			_wheelCollider.GetWorldPose(out _worldPos, out _worldRot);
			if(_targetObj)
			{
				_targetObj.position = _worldPos;
				_targetObj.rotation = _worldRot;
			}
		}
	}
}

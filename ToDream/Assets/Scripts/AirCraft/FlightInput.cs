using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightInput : BaseInput
{
	#region Fields
	
	protected float _speed = 0f; // 속도
	protected float _roll = 0f; // 세로축 (Y)
	protected float _yaw = 0f; // 수직축 (Z)
	protected float _pitch = 0f; // 가로축 (X)
	
	#endregion
	
	#region Properties
	
	public float _Speed {get{return _speed;} set{_speed = value;} }
	public float _Roll { get{return _roll;} set{_roll = value;} }
	public float _Yaw { get{return _yaw;} set{_yaw = value;} }
	public float _Pitch { get{return _pitch;} set{_pitch = value;} }
	
	#endregion
	
	#region Unity Events
	
	public override void Update()
	{
		HandleInput();
	}
	
	#endregion
	
	#region Custom Methods
	
	protected override void HandleInput()
	{
		_speed = Input.GetAxis("Vertical");
		_yaw = Input.GetAxis("Horizontal");
		_roll = Input.GetAxis("Mouse X");
		_pitch = Input.GetAxis("Mouse Y");
	}
	
	#endregion
}

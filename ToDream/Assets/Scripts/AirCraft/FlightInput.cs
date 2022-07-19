using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightInput : MonoBehaviour
{
	#region Fields
	
	private const int _MaxFlapIncrements = 2;
	
	protected float _speed = 0f; // 속도
	protected float _throttleSpeed = 0.06f;
	private float _stickyThrottle;
	
	protected float _roll = 0f; // 세로축
	protected float _yaw = 0f; // 수직축
	protected float _pitch = 0f; // 가로축
	
	protected int _flaps = 0;
	
	#endregion
	
	#region Properties
	
	public float _Speed {get{return _speed;} }
	public float _StickyThrottle { get { return _stickyThrottle; } }
	
	public float _Roll { get{return _roll;} }
	public float _Yaw { get{return _yaw;} }
	public float _Pitch { get{return _pitch;} }
	
	public float _Flaps { get{return _flaps;} }
	public float _NormalizedFlaps { get{return (float)_flaps / _MaxFlapIncrements;} }
	
	#endregion
	
	#region Unity Events
	
	public virtual void Start()
	{
		
	}
	
	public virtual void Update()
	{
		HandleInput();
	}
	
	#endregion
	
	#region Custom Methods
	
	protected virtual void HandleInput()
	{
		_speed = Input.GetAxis("Vertical");
		_roll = Input.GetAxis("Horizontal");
		_yaw = Input.GetAxis("Mouse X");
		_pitch = Input.GetAxis("Mouse Y");
		
		SpeedControl();
	}
	
	protected virtual void SpeedControl()
	{
		_stickyThrottle = _stickyThrottle + (_speed * _throttleSpeed * Time.deltaTime);
		_stickyThrottle = Mathf.Clamp01(_stickyThrottle);
	}
	
	#endregion
}

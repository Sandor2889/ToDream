using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlightControlable : Controlable
{
	#region Constants
	private const float _poundToKilos = 0.453592f;
	private const float _metersToFeet = 3.28084f;
	#endregion
	
	private Rigidbody _rb;
	private FlightCharacteristics _characteristics;
	
	public float _weight = 800f;
	public Transform _centerOfGravity;
	
	[Header("Engines")]
	public List<FlightEngine> _engines = new List<FlightEngine>();
	
	[Header("Wheels")]
	public List<FlightWheel> _wheels = new List<FlightWheel>();
	
	[Header("Ground")]
	[SerializeField] private LayerMask _groundMask;
	
	[Header("State")]
	public FlightState _state = FlightState.GROUNDED;
	
	
	
	[SerializeField]
	private bool _isGrounded = true;
	
	private const int _MaxFlapIncrements = 2;
	
	protected float _speed = 0f; // 속도
	protected float _throttleSpeed = 0.06f;
	private float _stickyThrottle;
	
	protected float _roll = 0f; // 세로축
	protected float _yaw = 0f; // 수직축
	protected float _pitch = 0f; // 가로축
	
	protected override void Awake()
	{
		base.Awake();
		_characteristics = GetComponent<FlightCharacteristics>();
	}
	
	protected override void Start()
	{
		if(GetComponent<Rigidbody>())
		{
			_rb = GetComponent<Rigidbody>();
		}
		else
		{
			_rb = gameObject.AddComponent<Rigidbody>();
		}
		
		if(_rb)
		{
			_rb.mass = _weight * _poundToKilos;
			_rb.centerOfMass = _centerOfGravity.localPosition;
			if(_characteristics)
			{
				_characteristics.InitCharacteristics(_rb);
			}
		}
		_state = FlightState.GROUNDED;
	}
	
	// 키보드 (w, a, s, d)
	public override void Move(Vector2 input)
	{
		_speed = input.y;
		_stickyThrottle = _stickyThrottle + (_speed * _throttleSpeed * Time.deltaTime);
		_stickyThrottle = Mathf.Clamp01(_stickyThrottle);
		// speed , roll
		
		
	}
	
	// 움직임
	public override void FixedMove()
	{
		if(_engines != null && _engines.Count > 0)
		{
			for(int i = 0; i < _engines.Count; i++)
			{
				_rb.AddForce(_engines[i].CalculateForce(_stickyThrottle));
			}
		}
		
		if(_characteristics)
		{
			_characteristics.UpdateChracteristics();
		}
	}
	
	// 마우스 (mouse X, mouse Y)
	public override void Rotate(Vector2 input)
	{
		// yaw , pitch
	}
	
	public override void Interact()
	{
		
	}
	
	public override void Boost(bool keydown)
	{
		
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		
	}
}

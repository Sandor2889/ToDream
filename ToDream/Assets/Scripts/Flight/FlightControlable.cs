using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlightControlable : Controlable
{
	FlightInput _input;
	
	#region Constants
	
	private const float _poundToKilos = 0.453592f;
	private const float _metersToFeet = 3.28084f;
	protected float _speed = 0f; // 속도 (w , s)
	protected float _throttleSpeed = 0.06f;
	private float _stickyThrottle;
	public float _StickThrottle {get{return _stickyThrottle;}}
	
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
	
	//[SerializeField]
	//private bool _isGrounded = false;
	
	protected override void Awake()
	{
		_characteristics = GetComponent<FlightCharacteristics>();
		_input = GetComponent<FlightInput>();
	}
	
	protected override void OnDisable()
	{
		_speed = 0f;
		_stickyThrottle = 0f;
		
		_state = FlightState.GROUNDED;
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
				_characteristics.InitCharacteristics(_rb, _input);
			}
		}
		_state = FlightState.GROUNDED;
	}
	
	// 키보드 (w, a, s, d)
	public override void Move()
	{
		if(_input._Speed.Equals(0)) return;
		// speed
		_speed = _input._Speed;
		_stickyThrottle = _stickyThrottle + (_speed * _throttleSpeed * Time.deltaTime);
		_stickyThrottle = Mathf.Clamp01(_stickyThrottle);
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
	public override void Rotate()
	{
		
	}
	
	public override void Interact()
	{
		_Controlable = FindObjectOfType<CharacterControlable>(true);
		
		_Controller.ChangeControlTarget(this, _Controlable);
	}
	
	public override void Boost(bool keydown)
	{
		
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		
	}
}

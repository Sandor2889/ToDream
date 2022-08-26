using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
	#region Fields
	
	private CarInput _input;
	
	[Header("Head")]
	[SerializeField] private Transform _head;
	
	[Header("Rigidbody")]
	[SerializeField] private Rigidbody _motorRB;
	[SerializeField] private Rigidbody _carColliderRB;
	
	[Header("Spec")]
	[SerializeField] protected float _fwdSpeed;
	[SerializeField] protected float _boostSpeed;
	[SerializeField] protected float _revSpeed;
	[SerializeField] protected float _turnSpeed;
	[SerializeField] protected float _driftTurnSpeed;
	[SerializeField] protected LayerMask _groundLayer;
	[SerializeField] protected LayerMask _UnDriveLayer;
	[SerializeField] private float _alignToGroundTime;
	[SerializeField] protected float _normalDrag;
	[SerializeField] protected float _modifiedDrag;
	
	private float _speed;
	private float _yInput;
	private float _newRotation;
	private Quaternion _toRotateTo;
	private bool _isGrounded;
	private RaycastHit _hit;
	private bool _firstSpawn = true;
	
	#endregion
	
	#region Properties
	
	public Rigidbody _MotorRB { get{return _motorRB;} }
	public Rigidbody _CarColliderRB { get{return _carColliderRB;} }
	
	#endregion
	
	#region Unity Events
	
	protected override void Awake()
	{
		_input = GetComponent<CarInput>();
	}
	
	protected override void OnEnable()
	{
		if(!_firstSpawn)
		{
			_motorRB.transform.parent = null;
			_carColliderRB.transform.parent = null;
			_normalDrag = _motorRB.drag;
		}
		else
		{
			_firstSpawn = false;
		}
	}
	
	protected override void OnDisable()
	{
		
	}
	
	#endregion
	
	#region Custom Methods
	
	public override void Move()
	{
		_yInput = _input._Vertical * _speed;
		
		_newRotation = _input._Horizontal * _turnSpeed * Time.deltaTime * _yInput;
		if(_isGrounded) transform.Rotate(0, _newRotation, 0, Space.World);
		
		transform.position = _motorRB.transform.position;
		
		
		_isGrounded = Physics.Raycast(transform.position, -transform.up, out _hit, 1.5f, _groundLayer);
		
		// 전방 체크
		Ray ray = new Ray(_head.transform.position, transform.forward);
		RaycastHit hit;
		bool test = Physics.Raycast(ray, out hit, 4f, _UnDriveLayer);
		if(test)
		{
			if(_yInput > 0) _yInput = 0;
		}
		_speed = _input._Vertical > 0 ? _fwdSpeed : _revSpeed;
		_motorRB.drag = _isGrounded ? _normalDrag : _modifiedDrag;
	}
	
	public override void FixedMove()
	{
		if(_isGrounded)
		{
			_motorRB.AddForce(transform.forward * _yInput, ForceMode.Acceleration);
			_toRotateTo = Quaternion.FromToRotation(transform.up, _hit.normal) * transform.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, _toRotateTo, _alignToGroundTime * Time.deltaTime);
		}
		else
		{
			_motorRB.AddForce(transform.up * -60f);
		}
		_carColliderRB.MoveRotation(transform.rotation);
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		if(keydown)
		{
			_newRotation = _input._Horizontal * _turnSpeed * Time.deltaTime * _yInput;
			if(_isGrounded) transform.Rotate(0, _newRotation, 0, Space.World);
		}
	}
	
	public override void Boost(bool keydown)
	{
		if(keydown)
		{
			_speed = _boostSpeed;
		}
	}
	
	public override void Interact(){ }
	
	public override void Rotate() { }
	
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
	#region Fields
	
	private CarInput _input;
	
	[Header("Rigidbody")]
	[SerializeField] private Rigidbody _motorRB;
	[SerializeField] private Rigidbody _carColliderRB;
	
	[Header("Spec")]
	[SerializeField] protected float _fwdSpeed;
	[SerializeField] protected float _revSpeed;
	[SerializeField] protected float _turnSpeed;
	[SerializeField] protected LayerMask _groundLayer;
	[SerializeField] private float _alignToGroundTime;
	[SerializeField] protected float _normalDrag;
	[SerializeField] protected float _modifiedDrag;
	
	private float _speed;
	private float _yInput;
	private float _newRotation;
	private Quaternion _toRotateTo;
	private bool _isGrounded;
	
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
		_motorRB.transform.parent = null;
		_carColliderRB.transform.parent = null;
		_normalDrag = _motorRB.drag;
	}
	
	protected override void OnDisable()
	{
		
	}
	
	#endregion
	
	#region Custom Methods
	
	public override void Move()
	{
		_speed = _input._Vertical > 0 ? _fwdSpeed : _revSpeed;
		_motorRB.drag = _isGrounded ? _normalDrag : _modifiedDrag;
		_yInput = _input._Vertical * _speed;
		
		_newRotation = _input._Horizontal * _turnSpeed * Time.deltaTime * _yInput;
		
		RaycastHit hit;
		_isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, _groundLayer);
		
		_toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
		transform.rotation = Quaternion.Slerp(transform.rotation, _toRotateTo, _alignToGroundTime * Time.deltaTime);
		
		if(_isGrounded) transform.Rotate(0, _newRotation, 0, Space.World);
		
		transform.position = _motorRB.transform.position;
	}
	
	public override void FixedMove()
	{
		if(_isGrounded)
		{
			_motorRB.AddForce(transform.forward * _yInput, ForceMode.Acceleration);
		}
		else
		{
			_motorRB.AddForce(transform.up * -30f);
		}
		
		_carColliderRB.MoveRotation(transform.rotation);
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		
	}
	
	public override void Boost(bool keydown)
	{
		Debug.Log("boost");
	}
	
	public override void Interact()
	{
		
	}
	
	public override void Rotate() { }
	
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
	[SerializeField] private Rigidbody _motorRB;
	[SerializeField] private Rigidbody _carColliderRB;
	[SerializeField] protected float _fwdSpeed;
	[SerializeField] protected float _revSpeed;
	[SerializeField] protected float _turnSpeed;
	[SerializeField] protected LayerMask _groundLayer;
	
	private float _newRotation;
	private float _yInput;
	private Quaternion _toRotateTo;
	private bool _isGrounded;
	
	[SerializeField] private float _alignToGroundTime;
	
	public float _normalDrag;
	public float _modifiedDrag;
	
	protected override void OnEnable()
	{
		_motorRB.transform.parent = null;
		_carColliderRB.transform.parent = null;
		_normalDrag = _motorRB.drag;
		//_newRotation = transform.eulerAngles.y;
	}
	
	public override void Move(Vector2 input)
	{
		input.y *= input.y > 0 ? _fwdSpeed : _revSpeed;
		_motorRB.drag = _isGrounded ? _normalDrag : _modifiedDrag;
		_yInput = input.y;
		
		_newRotation = input.x * _turnSpeed * Time.deltaTime * input.y;
		
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
	
	public override void Rotate(Vector2 input)
	{
		
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		
	}
	
	public override void Boost(bool keydown)
	{
		
	}
	
	public override void Interact()
	{
		_Controlable = FindObjectOfType<CharacterControlable>(true);
		_motorRB.transform.SetParent(this.transform);
		_motorRB.transform.localEulerAngles = Vector3.zero;
		_Controller.ChangeControlTarget(this, _Controlable);
	}
}

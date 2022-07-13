using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
	[SerializeField] private Rigidbody _motorRb;
	
	private float _newRotation;
	
	[SerializeField] private float _fwdSpeed;
	[SerializeField] private float _revSpeed;
	[SerializeField] private float _turnSpeed;
	[SerializeField] private LayerMask _groundLayer;
	
	private bool _isGrounded;
	
	private void OnEnable()
	{
		_motorRb.transform.parent = null;
		//_newRotation = transform.eulerAngles.y;
	}
	
	public override void Move(Vector2 input)
	{
		input.y *= input.y > 0 ? _fwdSpeed : _revSpeed;
		
		_newRotation = input.x * _turnSpeed * Time.deltaTime * input.y;
		
		RaycastHit hit;
		_isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, _groundLayer);
		
		if(_isGrounded) transform.Rotate(0, _newRotation, 0, Space.World);
		
		if(_isGrounded)
		{
			_motorRb.AddForce(transform.forward * input.y, ForceMode.Acceleration);
		}
		else
		{
			_motorRb.AddForce(transform.up * -30f);
		}
		
		transform.position = _motorRb.transform.position;
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
		_motorRb.transform.SetParent(this.transform);
		_motorRb.transform.localEulerAngles = Vector3.zero;
		_Controller.ChangeControlTarget(this, _Controlable);
	}
}

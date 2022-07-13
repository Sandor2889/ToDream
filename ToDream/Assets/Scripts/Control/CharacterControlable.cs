using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControlable : Controlable
{
	PlayerStateMachine _machine;
	CharacterController _characterController;
	Vector2 _currentMovementInput;
	Vector3 _appliedMovement;
	Vector3 _direction;
	bool _isMovementPressed;
	bool _isJumpPressed;
	bool _isRunPressed;
	float _eulerAngleY;
	
	public CharacterController _CharacterController { get{return _characterController;} }
	public bool _IsMovementPressed { get{return _isMovementPressed;} }
	public bool _IsJumpPressed { get{return _isJumpPressed;} }
	public bool _IsRunPressed { get{return _isRunPressed;} }
	public Vector2 _CurrentMovementInput { get{return _currentMovementInput;} }
	
	private void OnEnable()
	{
		_eulerAngleY = this.transform.eulerAngles.y;
	}
	
	private void Start()
	{
		_machine = GetComponent<PlayerStateMachine>();
		_characterController = GetComponent<CharacterController>();
	}
	
	public override void Move(Vector2 input)
	{
		if(input != Vector2.zero)
		{
			_isMovementPressed = true;
			_currentMovementInput = input.normalized;
		}
		else _isMovementPressed = false;
		
		_direction = transform.rotation * new Vector3(_machine._AppliedMovementX, 0, _machine._AppliedMovementZ);
		_characterController.Move(new Vector3(_direction.x, _machine._AppliedMovementY, _direction.z) * 10 * Time.deltaTime);
	}
	
	public override void Rotate(Vector2 input)
	{
		_eulerAngleY += input.x * 3;
		transform.rotation = Quaternion.Euler(0, _eulerAngleY, 0);
	}
	
	public override void JumpOrBreak(bool keydown)
	{
		_isJumpPressed = keydown;
	}
	
	public override void Boost(bool keydown)
	{
		if(_isMovementPressed) _isRunPressed = keydown;
	}
	
	public override void Interact()
	{
		_Controlable = FindObjectOfType<VehicleControlable>(true);
		Rigidbody rb = _Controlable.GetComponentInChildren<Rigidbody>();
		rb.isKinematic = true;
		_Controller.ChangeControlTarget(this, _Controlable);
		rb.isKinematic = false;
	}
}

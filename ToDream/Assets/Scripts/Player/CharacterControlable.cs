using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControlable : Controlable
{
	PlayerStateMachine _machine;
	CharacterInput _input;
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
	
	protected override void OnEnable()
	{
		_eulerAngleY = this.transform.eulerAngles.y;
	}
	
	protected override void Start()
	{
		_machine = GetComponent<PlayerStateMachine>();
		_characterController = GetComponent<CharacterController>();
		_input = GetComponent<CharacterInput>();
	}
	
	public override void Move()
	{
		if(_input._Vertical != 0 || _input._Horizontal != 0)
		{
			_isMovementPressed = true;
			_currentMovementInput = new Vector2(_input._Horizontal, _input._Vertical).normalized;
		}
		else _isMovementPressed = false;
		
		_direction = transform.rotation * new Vector3(_machine._AppliedMovementX, 0, _machine._AppliedMovementZ);
		
	}
	
	public override void FixedMove()
	{
		_characterController.Move(new Vector3(_direction.x, _machine._AppliedMovementY, _direction.z) * 10 * Time.fixedDeltaTime);
	}
	
	public override void Rotate()
	{
		_eulerAngleY += _input._MouseX * 3;
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
		
	}
}

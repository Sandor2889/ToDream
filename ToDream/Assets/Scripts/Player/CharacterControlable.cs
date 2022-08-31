using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControlable : Controlable
{
	#region Fields
	
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
	
	// Check Collision Time
	float _startTime = 0;
	float _time;
	float _endTime = 5;
	
	public CharacterController _CharacterController { get{return _characterController;} }
	public bool _IsMovementPressed { get{return _isMovementPressed;} }
	public bool _IsJumpPressed { get{return _isJumpPressed;} }
	public bool _IsRunPressed { get{return _isRunPressed;} }
	public Vector2 _CurrentMovementInput { get{return _currentMovementInput;} }
	
	#endregion
	
	#region Unity Events
	
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
	
	#endregion
	
	
	#region Custom Methods
	public override void Move()
	{
		// 움직이면서 NPC대화시 계속 움직이는 버그 발생으로 여기서 조건 추가 (UIManager.IsTalking())
		// 이전 조건에서는 _direction의 값이 +값이 주어진 상태로 update가 막혀 계속 전진하는 현상이 일어났음
		if((_input._Vertical != 0 || _input._Horizontal != 0) && !UIManager.IsTalking())
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
	
	protected void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if((int)_CharacterController.collisionFlags == 5 && hit.collider.CompareTag("Wall"))
		{
			_time += Time.deltaTime;
			if(_time >= _endTime)
			{
				Transform target = RespawnManager._Instance.Respawn(this.transform);
				this.transform.position = target.position;
				
				_time = _startTime;
				return;
			}
		}
		else if((int)_CharacterController.collisionFlags != 5 && !hit.collider.CompareTag("Wall"))
		{
			_time = _startTime;
		}
		if(_CharacterController.collisionFlags == CollisionFlags.Below && hit.collider.CompareTag("Water"))
		{
			Debug.Log("Respawn");
		}
	}
	
	#endregion
}

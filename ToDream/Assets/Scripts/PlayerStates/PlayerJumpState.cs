using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
	public PlayerJumpState
	(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		_IsMainState = true;
	}
	
	public override void EnterState()
	{
		InitializeSubState();
		_Ctx._IsJumping = true;
		HandleJump();
	}
	
	public override void UpdateState()
	{
		HandleGravity();
		CheckSwitchState();
	}
	
	public override void ExitState()
	{
		_Ctx._IsJumping = false;
	}
	
	public override void CheckSwitchState()
	{
		if(_Ctx._CharacterController.isGrounded)
		{
			SwitchState(_Factory.Grounded());
		}
	}
	
	public override void InitializeSubState()
	{
		if(!_Ctx._IsMovementPressed && !_Ctx._IsRunPressed)
		{
			SetSubState(_Factory.Idle());
		}
		else if(_Ctx._IsMovementPressed && !_Ctx._IsRunPressed)
		{
			SetSubState(_Factory.Walk());
		}
		else if(_Ctx._IsMovementPressed && _Ctx._IsRunPressed)
		{
 			SetSubState(_Factory.Run());
		}
	}
	
	public void HandleGravity()
	{
		bool isFalling = _Ctx._CurrentMovementY <= 0.0f;
		float multiplier = 0.5f;
		
		if(isFalling)
		{
			float previousYVelocity = _Ctx._CurrentMovementY;
			_Ctx._CurrentMovementY = _Ctx._CurrentMovementY + (_Ctx._JumpGravities[1] * Time.deltaTime);
			_Ctx._AppliedMovementY = (previousYVelocity + _Ctx._CurrentMovementY) * multiplier;
		}
		else
		{
			float previousYVelocity = _Ctx._CurrentMovementY;
			_Ctx._CurrentMovementY = _Ctx._CurrentMovementY + (_Ctx._JumpGravities[1] * Time.deltaTime);
			_Ctx._AppliedMovementY = (previousYVelocity + _Ctx._CurrentMovementY) * multiplier;
		}
	}
	
	private void HandleJump()
	{
		_Ctx._CurrentMovementY = _Ctx._InitialJumpVelocities[1];
		_Ctx._AppliedMovementY = _Ctx._InitialJumpVelocities[1];
	}
}

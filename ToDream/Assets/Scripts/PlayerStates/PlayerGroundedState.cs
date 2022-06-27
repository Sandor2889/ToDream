using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
	public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		_IsRootState = true;
	}
	
	public override void EnterState()
	{
		InitializeSubState();
		_Ctx._AppliedMovementY = _Ctx._GroundedGravity;
	}
	
	public override void UpdateState()
	{
		CheckSwitchState();
	}
	
	public override void CheckSwitchState()
	{
		if(_Ctx._IsJumpPressed)
		{
			SwitchState(_Factory.Jump());
		}
		else if(!_Ctx._CharacterController.isGrounded)
		{
			SwitchState(_Factory.Fall());
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
		else
		{
			SetSubState(_Factory.Run());
		}
	}

	public override void ExitState() { }
}

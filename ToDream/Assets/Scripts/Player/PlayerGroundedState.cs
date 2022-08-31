using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
	public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		_IsMainState = true;
	}
	
	public void HandleGravity()
	{
		_Ctx._CurrentMovementY = _Ctx._Gravity;
		_Ctx._AppliedMovementY = _Ctx._Gravity;
	}
	
	public override void EnterState()
	{
		InitializeSubState();
		HandleGravity();
	}
	
	public override void UpdateState()
	{
		CheckSwitchState();
	}
	
	public override void CheckSwitchState()
	{
		// 땅에 있는 상태에서 점프키를 누르면 점프 상태로 바뀜
		if(_Ctx._IsJumpPressed)
		{
			SwitchState(_Factory.Jump());
		}
		// 땅에 있지 않은 상태에서 점프가 눌리지 않은 상태이면 떨어지는 상태로 바뀜
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
		else if (_Ctx._IsMovementPressed && _Ctx._IsRunPressed)
		{
			SetSubState(_Factory.Run());
		}
	}

	public override void ExitState() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
	public PlayerFallState
	(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		_IsMainState = true;
	}
	
	public override void EnterState()
	{
		InitializeSubState();
	}
	
	public override void UpdateState()
	{
		HandleGravity();
		CheckSwitchState();
	}
	
	public override void ExitState()
	{
		
	}
	
	public override void CheckSwitchState()
	{
		// 플레이어가 땅에 있으면 Grounded 상태로 바뀜
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
		else if (_Ctx._IsMovementPressed && _Ctx._IsRunPressed)
		{
			SetSubState(_Factory.Run());
		}
	}
	
	public void HandleGravity()
	{
		float previousYVelocity = _Ctx._CurrentMovementY;
		_Ctx._CurrentMovementY = _Ctx._CurrentMovementY + _Ctx._Gravity * Time.deltaTime;
		_Ctx._AppliedMovementY = (previousYVelocity + _Ctx._CurrentMovementY) * 0.1f;
	}
}

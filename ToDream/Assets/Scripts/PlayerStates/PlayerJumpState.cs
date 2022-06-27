using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
	public PlayerJumpState
	(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory)
	{
		_IsRootState = true;
		InitializeSubState();
	}
	
	public override void EnterState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
	
	public override void UpdateState()
	{
		
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
		
	}
}

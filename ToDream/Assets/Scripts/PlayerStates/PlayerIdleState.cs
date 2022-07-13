using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
	public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory) { }
	
	public override void EnterState()
	{
		_Ctx._Animator.SetBool(_Ctx._IsWalkingHash, false);
		_Ctx._Animator.SetBool(_Ctx._IsRunningHash, false);
		_Ctx._AppliedMovementX = 0;
		_Ctx._AppliedMovementZ = 0;
	}
	
	public override void UpdateState()
	{
		CheckSwitchState();
	}
	
	public override void CheckSwitchState()
	{
		if(_Ctx._IsMovementPressed && _Ctx._IsRunPressed)
		{
			SwitchState(_Factory.Run());
		}
		else if(_Ctx._IsMovementPressed)
		{
			SwitchState(_Factory.Walk());
		}
	}
	
	public override void ExitState() { }
	
	public override void InitializeSubState() { }
}

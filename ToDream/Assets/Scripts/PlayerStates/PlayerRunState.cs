using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
	public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	: base(currentContext, playerStateFactory) { }
	
	public override void EnterState()
	{
		
	}
	
	public override void UpdateState()
	{
		_Ctx._AppliedMovementX = _Ctx._CurrentMovementInput.x * _Ctx._RunMultiplier;
		_Ctx._AppliedMovementZ = _Ctx._CurrentMovementInput.y * _Ctx._RunMultiplier;
		CheckSwitchState();
	}

	public override void CheckSwitchState()
	{
		if (!_Ctx._IsMovementPressed)
		{
			SwitchState(_Factory.Idle());
		}
		else if (_Ctx._IsMovementPressed && !_Ctx._IsRunPressed)
		{
			SwitchState(_Factory.Walk());
		}
	}

	public override void InitializeSubState() { }
	
	public override void ExitState() { }
	
	
}

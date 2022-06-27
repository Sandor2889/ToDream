using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
	Idle,
	Walk,
	Run,
	Grounded,
	Jump,
	Fall
}

public class PlayerStateFactory
{
	PlayerStateMachine _context;
	Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
	
	public PlayerStateFactory(PlayerStateMachine currentContext)
	{
		_context = currentContext;
		
		_states[PlayerStates.Idle] = new PlayerIdleState(_context, this);
		_states[PlayerStates.Walk] = new PlayerWalkState(_context, this);
		_states[PlayerStates.Run] = new PlayerRunState(_context, this);
		_states[PlayerStates.Jump] = new PlayerJumpState(_context, this);
		_states[PlayerStates.Grounded] = new PlayerGroundedState(_context, this);
		_states[PlayerStates.Fall] = new PlayerFallState(_context, this);
	}
	
	public PlayerBaseState Idle()
	{
		return _states[PlayerStates.Idle];
	}
	
	public PlayerBaseState Walk()
	{
		return _states[PlayerStates.Walk];
	}
	
	public PlayerBaseState Run()
	{
		return _states[PlayerStates.Run];
	}
	
	public PlayerBaseState Jump()
	{
		return _states[PlayerStates.Jump];
	}
	
	public PlayerBaseState Grounded()
	{
		return _states[PlayerStates.Grounded];
	}
	
	public PlayerBaseState Fall()
	{
		return _states[PlayerStates.Fall];
	}
}

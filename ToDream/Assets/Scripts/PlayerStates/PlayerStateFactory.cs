using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코드 최적화
// Dictionary의 Key 값이 State 이기 때문에 Enum 을 사용할 수 있음
// 하지만 Key 값을 Enum 으로 두면 int 와 비교했을 때 너무나도 손해 ( GC )
// TryGetValue 기준(1번) int 는 가비지 0 , Enum 은 가비지 60B
// 10만번 돌리면 5.7MB 차이남
// (int) 로 캐스팅 하는 것은 가독성이나 작업량에서 손해이기에 자동화를 해줌

public enum PlayerStates
{
	Idle,
	Walk,
	Run,
	Grounded,
	Jump,
	Fall
}

public struct PlayerStateEnumComparer : IEqualityComparer<PlayerStates>
{
	public bool Equals(PlayerStates A, PlayerStates B)
	{
		return A == B;
	}
	
	public int GetHashCode(PlayerStates state)
	{
		return (int)state;
	}
}

public class PlayerStateFactory
{
	PlayerStateMachine _context;
	Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>(new PlayerStateEnumComparer());
	
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

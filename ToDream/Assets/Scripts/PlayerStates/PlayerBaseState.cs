using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
	private bool _isRootState = false;
	private PlayerStateMachine _ctx;
	private PlayerStateFactory _factory;
	private PlayerBaseState _currentSubState;
	private PlayerBaseState _currentSuperState;
	
	// get, set variables
	protected bool _IsRootState { set{_isRootState = value; } }
	protected PlayerStateMachine _Ctx { get{return _ctx; } }
	protected PlayerStateFactory _Factory { get {return _factory;} }
	
	public PlayerBaseState
	(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	{
		_ctx = currentContext;
		_factory = playerStateFactory;
	}
	
	public abstract void EnterState();
	
	public abstract void CheckSwitchState();
	
	public abstract void ExitState();
	
	public abstract void InitializeSubState();
	
	public abstract void UpdateState();
	
	protected void SwitchState(PlayerBaseState newState)
	{
		// 기존 상태 나가기
		ExitState();
		
		// 새로운 상태 설정
		newState.EnterState();
		
		if(_isRootState)
		{
			_ctx._CurrentState = newState;
		}
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
	{
		_currentSubState = newSubState;
		newSubState.SetSuperState(this);
	}

	public void UpdateStates()
    {
		UpdateState();
		if(_currentSubState != null)
        {
			_currentSubState.UpdateStates();
        }
    }
}

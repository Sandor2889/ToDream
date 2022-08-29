using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
	private bool _isMainState = false;
	private PlayerStateMachine _ctx;
	private PlayerStateFactory _factory;
	private PlayerBaseState _currentMainState; // 메인 상태
	private PlayerBaseState _currentSubState; // 서브 상태

	// get, set variables
	protected bool _IsMainState { set{_isMainState = value; } }
	protected PlayerStateMachine _Ctx { get{return _ctx; } }
	protected PlayerStateFactory _Factory { get {return _factory;} }
	
	public PlayerBaseState
	(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
	{
		_ctx = currentContext;
		_factory = playerStateFactory;
	}

	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void ExitState();
	public abstract void CheckSwitchState();
	public abstract void InitializeSubState();
	
	protected void SwitchState(PlayerBaseState newState)
	{
		// 기존 상태 나가기
		ExitState();
		
		// 새로운 상태 설정
		newState.EnterState();
		
		if(_isMainState)
		{
			_ctx._CurrentState = newState;
		}
        else if(_currentMainState != null)
        {
            _currentMainState.SetSubState(newState);
        }
    }

    protected void SetMainState(PlayerBaseState newMainState)
    {
        _currentMainState = newMainState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
	{
 		if (_currentSubState != null) _currentSubState.ExitState();
		_currentSubState = newSubState;
		_currentSubState.EnterState();
		if(_currentMainState == null) _currentSubState.SetMainState(this);	
	}

	public void UpdateStates()
    {
		UpdateState();
		if(_currentSubState != null)
        {
			_currentSubState.UpdateState();
        }
    }
}

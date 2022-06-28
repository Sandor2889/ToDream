using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State Pattern 을 적용함에 따라
// 스크립트의 길이가 줄어들고 각 상태별로 구분하여 가독성이 좋아짐
// 상태가 추가될 때 새로운 스크립트를 만들어 상태를 추가해주면 되기에 확장성이 좋아짐

public class PlayerStateMachine : MonoBehaviour
{
	CharacterController _characterController;
	PlayerBaseState _currentState; // Main State
	PlayerStateFactory _states;
	Animator _animator;
	
	
	// move variables
	Vector2 _currentMovementInput;
	Vector3 _currentMovement;
	Vector3 _appliedMovement;
	
	float _runMultiplier = 1.5f;
	
	bool _isMovementPressed;
	bool _isRunPressed;
	
	
	// gravity variables
	float _gravity = -9.8f;
	
	
	// jumping variables
	bool _isJumpPressed = false;
	float _initialJumpVelocity;

	float _maxJumpHeight = 4.0f;
	float _maxJumpTime = .75f;
	bool _isJumping = false;
	int _isJumpingHash;
	int _jumpCountHash;
	
	Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
	Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
	
	// get, set variables
	public CharacterController _CharacterController { get{return _characterController;} }
	public PlayerBaseState _CurrentState { get{return _currentState;} set{_currentState = value;} }
	public Animator _Animator { get{return _animator;} }
	public float _CurrentMovementY { get{return _currentMovement.y;} set{_currentMovement.y = value;} }
	public float _AppliedMovementX { get{return _appliedMovement.x;} set{_appliedMovement.x = value;} }
	public float _AppliedMovementY { get{return _appliedMovement.y;} set{_appliedMovement.y = value;} }
	public float _AppliedMovementZ { get{return _appliedMovement.z;} set{_appliedMovement.z = value;} }
	public Vector2 _CurrentMovementInput { get{return _currentMovementInput;} }
	public float _RunMultiplier { get{return _runMultiplier;} }
	public bool _IsJumping { get{return _isJumping;} set{_isJumping = value;} }

	public bool _IsMovementPressed { get{return _isMovementPressed;} }
	public bool _IsRunPressed { get{return _isRunPressed;} }
	public bool _IsJumpPressed { get{return _isJumpPressed;} }
	
	public float _Gravity { get{return _gravity;} }
	
	public Dictionary<int, float> _InitialJumpVelocities { get{return _initialJumpVelocities;} }
	public Dictionary<int, float> _JumpGravities { get{return _jumpGravities;} }
	
	
	private void Awake()
	{
		// Components
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();

		float timeToApex = _maxJumpTime / 2;
		float initialGravity = (-0.5f * _maxJumpHeight) / timeToApex;
		_initialJumpVelocity = (0.5f * _maxJumpHeight) / (timeToApex * 2);
		_InitialJumpVelocities.Add(1, _initialJumpVelocity);
		_jumpGravities.Add(1, initialGravity);
		
		// setup state
		_states = new PlayerStateFactory(this);
		_currentState = _states.Grounded();
		_currentState.EnterState();
	}
	
	private void Start()
	{
		_characterController.Move(_appliedMovement * Time.deltaTime);
	}
	
	private void Update()
	{
		_currentMovementInput.x = Input.GetAxisRaw("Horizontal");
		_currentMovementInput.y = Input.GetAxisRaw("Vertical");

		if (_currentMovementInput != Vector2.zero)
        {
			_isMovementPressed = true;
		}
		else
        {
			_isMovementPressed = false;
        }

		if (_isMovementPressed && Input.GetKey(KeyCode.LeftShift))
		{
			_isRunPressed = true;
		}
		else
		{
			_isRunPressed = false;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_isJumpPressed = true;
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			_isJumpPressed = false;
		}
        
		_currentState.UpdateStates();
		_characterController.Move(_appliedMovement * 10 * Time.deltaTime);
		
		//HandleRotation();
	}
	
	private void HandleRotation()
	{
		
	}
}

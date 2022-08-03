using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State Pattern 을 적용함에 따라
// 스크립트의 길이가 줄어들고 각 상태별로 구분하여 가독성이 좋아짐
// 상태가 추가될 때 새로운 스크립트를 만들어 상태를 추가해주면 되기에 확장성이 좋아짐

public class PlayerStateMachine : MonoBehaviour
{
	public bool test = false;
	
	CharacterControlable _char;
	
	// State
	PlayerBaseState _currentState; // main State
	PlayerStateFactory _states;
	
	// Move variables
	Vector3 _currentMovement;
	Vector3 _appliedMovement;
	float _runMultiplier = 1.5f;
	
	// gravity variables
	float _gravity = -9.8f;
	
	// jumping variables
	float _initialJumpVelocity;
	float _maxJumpHeight = 4.0f;
	float _maxJumpTime = .75f;
	bool _isJumping = false;
	Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
	Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
	
	// Animation
	Animator _animator;
	readonly int _isWalkingHash = Animator.StringToHash("isWalking");
	readonly int _isRunningHash = Animator.StringToHash("isRunning");
	readonly int _isJumpingHash = Animator.StringToHash("isJumping");
	readonly int _isFallingHash = Animator.StringToHash("isFalling");

	// get, set variables
	public PlayerBaseState _CurrentState { get{return _currentState;} set{_currentState = value;} }
	
	public float _CurrentMovementY { get{return _currentMovement.y;} set{_currentMovement.y = value;} }
	public float _AppliedMovementX { get{return _appliedMovement.x;} set{_appliedMovement.x = value;} }
	public float _AppliedMovementY { get{return _appliedMovement.y;} set{_appliedMovement.y = value;} }
	public float _AppliedMovementZ { get{return _appliedMovement.z;} set{_appliedMovement.z = value;} }
	public float _RunMultiplier { get{return _runMultiplier;} }
	
	public bool _IsJumping { get{return _isJumping;} set{_isJumping = value;} }
	public float _Gravity { get{return _gravity;} }
	public Dictionary<int, float> _InitialJumpVelocities { get{return _initialJumpVelocities;} }
	public Dictionary<int, float> _JumpGravities { get{return _jumpGravities;} }
	
	public Animator _Animator { get{return _animator;} }
	public int _IsWalkingHash { get{return _isWalkingHash;} }
	public int _IsRunningHash { get{return _isRunningHash;} }
	public int _IsJumingHash { get{return _isJumpingHash;} }
	public int _IsFallingHash { get {return _isFallingHash;} }
	
	public CharacterController _CharacterController => _char._CharacterController;
	public Vector2 _CurrentMovementInput => _char._CurrentMovementInput;
	public bool _IsMovementPressed => _char._IsMovementPressed;
	public bool _IsRunPressed => _char._IsRunPressed;
	public bool _IsJumpPressed => _char._IsJumpPressed;
	
	private void Awake()
	{
		// Components
		_char = FindObjectOfType<CharacterControlable>();
		_animator = GetComponent<Animator>();

		float timeToApex = _maxJumpTime / 2;
		float initialGravity = (-0.5f * _maxJumpHeight) / timeToApex;
		_initialJumpVelocity = (0.5f * _maxJumpHeight) / (timeToApex * 2);
		_InitialJumpVelocities.Add(1, _initialJumpVelocity);
		_jumpGravities.Add(1, initialGravity);
		
		// setup state
		_states = new PlayerStateFactory(this);
	}
	
	private void OnEnable()
	{
		_currentState = _states.Grounded();
		_currentState.EnterState();
	}
	
	private void OnDisable()
	{
		_currentMovement = Vector3.zero;
		_appliedMovement = Vector3.zero;
	}
	
	private void Update()
	{
		_currentState.UpdateStates();
		
		test = _CharacterController.isGrounded;
	}
}
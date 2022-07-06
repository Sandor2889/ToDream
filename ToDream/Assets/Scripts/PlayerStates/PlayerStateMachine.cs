using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State Pattern 을 적용함에 따라
// 스크립트의 길이가 줄어들고 각 상태별로 구분하여 가독성이 좋아짐
// 상태가 추가될 때 새로운 스크립트를 만들어 상태를 추가해주면 되기에 확장성이 좋아짐

public class PlayerStateMachine : MonoBehaviour
{
	CharacterController _characterController;
	PlayerBaseState _currentState; // main State
	PlayerStateFactory _states;
	Animator _animator;
	PlayerMouseInput _mouseInput = new PlayerMouseInput();
	PlayerMoveInput _moveInput = new PlayerMoveInput();
	
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
	
	readonly int _isWalkingHash = Animator.StringToHash("isWalking");
	readonly int _isRunningHash = Animator.StringToHash("isRunning");
	readonly int _isJumpingHash = Animator.StringToHash("isJumping");
	readonly int _isFallingHash = Animator.StringToHash("isFalling");

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
	
	public int _IsWalkingHash { get{return _isWalkingHash;} }
	public int _IsRunningHash { get{return _isRunningHash;} }
	public int _IsJumingHash { get{return _isJumpingHash;} }
	public int _IsFallingHash { get {return _isFallingHash;} }
	
	public Transform _cam;
	public GameObject _car;
	
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
		_currentMovementInput.x = _moveInput._Horizontal;
		_currentMovementInput.y = _moveInput._Vertical;
		if(_currentMovementInput.x != 0 && _currentMovementInput.y != 0) _currentMovementInput = _currentMovementInput.normalized;
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

		if (Input.GetKeyDown(_moveInput._KeyJump))
		{
			_isJumpPressed = true;
		}
		else if(Input.GetKeyUp(_moveInput._KeyJump))
		{
			_isJumpPressed = false;
		}
		if(Input.GetKeyDown(_moveInput._KeyUI))
		{
			if(_car != null && _car.gameObject.activeSelf == false)
			{
				_car.transform.position = this.transform.position;
				_car.transform.rotation = this.transform.rotation;
				_car.gameObject.SetActive(true);
			}
		}
		
		transform.rotation = 
			Quaternion.Euler(0, _mouseInput.UpdateRotate(_mouseInput._MouseX, _mouseInput._MouseY), 0);
			
		
		_currentState.UpdateStates();
		Vector3 direction = transform.rotation * new Vector3(_appliedMovement.x, 0, _appliedMovement.z);
		
		_characterController.Move(new Vector3(direction.x, _appliedMovement.y, direction.z) * 10 * Time.deltaTime);
	}
}

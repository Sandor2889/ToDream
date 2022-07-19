using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
	Vector2 _keyboardInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	Vector2 _mouseInput => new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
	
	private void Update()
	{
		InputMoveAxis();
		InputRotateAxis();
		JumpOrBreak();
		Boost();
		InteractKeys();
	}
	
	private void FixedUpdate()
	{
		_ControlTarget.FixedMove();
	}
	
	private void InputMoveAxis()
	{
		_ControlTarget.Move(_keyboardInput);
	}
	
	private void InputRotateAxis()
	{
		_ControlTarget.Rotate(_mouseInput);
	}
	
	private void JumpOrBreak()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			_ControlTarget.JumpOrBreak(true);
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			_ControlTarget.JumpOrBreak(false);
		}
	}
	
	private void Boost()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			_ControlTarget.Boost(true);
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			_ControlTarget.Boost(false);
		}
	}
	
	private void InteractKeys()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			_ControlTarget.Interact();
		}
	}
}

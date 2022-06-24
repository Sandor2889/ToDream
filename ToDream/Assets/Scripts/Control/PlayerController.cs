using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
	protected void Update()
	{
		InputMoveAxis();
		InputRotateAxis();
	}
	
	private void InputMoveAxis()
	{
		_ControlTarget.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
	}
	
	private void InputRotateAxis()
	{
		_ControlTarget.Rotate(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
	}
}

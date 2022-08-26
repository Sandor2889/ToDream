using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
	[SerializeField] private VehicleWheelController _ui;
	[SerializeField] private Controlable _current;
	[SerializeField] private Controlable _next;
	
	#region Unity Events
	
	private void Awake()
	{
		_current = _ControlTarget;
	}
	
	private void Update()
	{
		InputMoveAxis();
		if(!_ui._VehicleWheelSelected)
		{
			InputRotateAxis();
		}
		JumpOrBreak();
		Boost();
		InteractKeys();
	}
	
	private void FixedUpdate()
	{
		_ControlTarget.FixedMove();
	}
	
	#endregion
	
	#region Custom Methods
	
	private void InputMoveAxis()
	{
		_ControlTarget.Move();
	}
	
	private void InputRotateAxis()
	{
		_ControlTarget.Rotate();
	}
	
	private void JumpOrBreak()
	{
		if(Input.GetKey(KeyCode.Space))
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
			// UI 를 띄운다. (토글)
			_ui._VehicleWheelSelected = true;
			
			_ui.gameObject.SetActive(true);
		}
		else if(Input.GetKeyUp(KeyCode.C))
		{
			// 토글
			_ui._VehicleWheelSelected = false;	
			_ui.gameObject.SetActive(false);

			// 마우스가 있던 버튼의 enum 값을 가져오자.
			VehicleType vehicleType = VehicleWheelController._vehicleType;
			GameObject vehicleObj = VehicleWheelController._currentVehicles[(int)vehicleType];	

			// 현재 상태와 같은 상태를 선택할경우 return
			if (_current._vehicleType == vehicleType || vehicleObj == null) { return; }
			Debug.Log(vehicleType);
			switch (vehicleType)
			{
				case VehicleType.None:
					_next = vehicleObj.GetComponent<CharacterControlable>();
					ChangeControlTarget(_current, _next);
					break;
				case VehicleType.Car:
					_next = vehicleObj.GetComponent<VehicleControlable>();
					ChangeControlTarget(_current, _next);
					break;
				case VehicleType.Air:
					_next = vehicleObj.GetComponent<FlightControlable>();
					ChangeControlTarget(_current, _next);
					break;
					case VehicleType.Boat:
					// 미구현
					return;
				default:
					Debug.Log("Nothing");
					return;
			}
			_current = _next;
			_next = null;
		}
	}
	
	#endregion
}

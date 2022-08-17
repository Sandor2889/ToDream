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
			
			string id = VehicleWheelController._vehicleID;
			_ui.gameObject.SetActive(false);
			
			// 마우스가 있던 버튼의 ID 값을 가져오자.
			switch(id)
			{
				case "None":
					Debug.Log("None");
					// 이미 None 인 경우 return
					if(_current._vehicleType == VehicleType.None) return;
					// 아닌 경우 None 으로 만들어주기
					else
					{
						_next = FindObjectOfType<CharacterControlable>(true);
						ChangeControlTarget(_current, _next);
					}
					break;
				case "Car":
					Debug.Log("Car");
					// 이미 Car 인 경우 return
					if(_current._vehicleType == VehicleType.Car) return;
					// 아닌 경우 Car 으로 만들어주기
					else
					{
						_next = FindObjectOfType<VehicleControlable>(true);
						ChangeControlTarget(_current, _next);
					}
					break;
				case "Air":
					Debug.Log("Air");
					if(_current._vehicleType == VehicleType.Air) return;
					else
					{
						_next = FindObjectOfType<FlightControlable>(true);
						ChangeControlTarget(_current, _next);
					}
					break;
				case "Boat":
					Debug.Log("Boat");
					
					break;
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

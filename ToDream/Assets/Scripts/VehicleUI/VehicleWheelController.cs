using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWheelController : MonoBehaviour
{
	private bool _vehicleWheelSelected = false;
	//public static string _vehicleID;
	public static VehicleType _vehicleType;
	public static GameObject[] _currentVehicles = new GameObject[4];

	public bool _VehicleWheelSelected { get{return _vehicleWheelSelected;} set{_vehicleWheelSelected = value;} }

    //void Update()
    //{   
    // switch(_vehicleType)
    // {
    // case VehicleType.None:
    //  // nothing
    //  break;
    // case VehicleType.Car:
    //  // car
    //  break;
    // case VehicleType.Air:
    //  // flight
    //  break;
    // case VehicleType.Boat:
    // 	// boat
    //  break;
    // }
    //}
}

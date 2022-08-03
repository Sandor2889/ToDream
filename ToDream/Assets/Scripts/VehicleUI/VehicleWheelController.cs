using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWheelController : MonoBehaviour
{
	private bool _vehicleWheelSelected = false;
	public static string _vehicleID;
	public bool _VehicleWheelSelected { get{return _vehicleWheelSelected;} set{_vehicleWheelSelected = value;} }
	
    void Update()
    {   
	    switch(_vehicleID)
	    {
	    case "None":
		    // nothing
		    break;
	    case "Car":
		    // car
		    break;
	    case "Flight":
		    // flight
		    break;
	    case "Boat":
	    	// boat
		    break;
	    }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField] private Controlable _controlTarget;
	protected Controlable _ControlTarget
	{
		set { _controlTarget = value; } 
		get { return _controlTarget; } 
	}
	
	
}

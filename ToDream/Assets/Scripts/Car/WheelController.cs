using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
	private CarInput _carInput;
	[SerializeField] private TrailRenderer[] _trails;
	
	private void Start()
	{
		_carInput = GetComponent<CarInput>();
	}
	
	private void Update()
	{
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flight Character", menuName = "Flight/Create New Flight Character")]
public class FlightCharacteristic : ScriptableObject
{
	[Header("Characteristics")]
	public float _MaxMPS = 200f;
	
	[Header("Lift")]
	public float _MaxLiftPower = 800f;
	public float _FlapLiftPower = 100f;
	
	[Header("Drag")]
	public float _DragFactor = 0.01f;
	public float _FlapDragFactor = 0.005f;
	
	[Header("Controls")]
	public float _PitchSpeed = 100f;
	public float _RollSpeed = 100f;
	public float _YawSpeed = 100f;
	public float _BankingSpeed = 100f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
	[Header("Audio Properties")]
	[SerializeField] private CarInput _input;
	
	[Header("Audio Sources")]
	[SerializeField] AudioSource _idle;
	
	[Header("Pitch")]
	[SerializeField] private float _defaultPitch = 1.0f;
	[SerializeField] private float _maxPitch = 1.2f;

	private void Update()
	{
		if(_input)
		{
			HandleAudio();
		}
	}
    
	private void HandleAudio()
	{
		if(_input._Vertical == 1)
		{
			_idle.pitch = _maxPitch;
		}
		else
		{
			_idle.pitch = _defaultPitch;
		}
	}
}

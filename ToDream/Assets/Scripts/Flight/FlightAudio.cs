using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAudio : MonoBehaviour
{
	[Header("Audio Properties")]
	[SerializeField] private FlightEngine _engine;
	[SerializeField] private FlightControlable _control;
	
	[Header("Audio Sources")]
	[SerializeField] AudioSource _idle;
	[SerializeField] AudioSource _fullThrottle;
	
	[Header("Pitch")]
	[SerializeField] private float _maxPitch = 1.2f;
	private float _defaultPitch;
	private float _finalPitch;
	private float _finalVolume;
	
	private void Start()
	{
		if(_fullThrottle)
		{
			_fullThrottle.volume = 0f;
		}
	}

	private void Update()
    {
	    if(_engine)
	    {
	    	HandleAudio();
	    }
    }
    
	private void HandleAudio()
	{
		float rpm = Mathf.InverseLerp(0f, _engine._engine._MaxRPM, _engine._CurrentRPM);
		_finalVolume = Mathf.Lerp(0f, 1f, rpm * 1.5f);
		_finalPitch = Mathf.Lerp(1f, _maxPitch, rpm);
		
		if(_fullThrottle && _idle)
		{
			_fullThrottle.volume = _finalVolume;
			_fullThrottle.pitch = _finalPitch;
			
			_idle.volume = 1 - (_control._StickThrottle);
		}
	}
}

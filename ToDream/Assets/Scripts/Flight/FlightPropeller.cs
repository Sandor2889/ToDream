using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPropeller : MonoBehaviour
{
	[Header("Propeller")]
	private float _minQuadRPM = 300f;
	private float _minSwapRPM = 600f;
	private float _minRotRPM = 40f;
	
	[SerializeField] private GameObject _main;
	[SerializeField] private GameObject _blurred;
	
	[SerializeField] private Texture2D _smoothBlur;
	[SerializeField] private Texture2D _hardBlur;
	
	private Renderer _propellerRenderer;
	
	private void Awake()
	{
		_propellerRenderer = _blurred.GetComponent<Renderer>();
	}
	
	private void Start()
	{
		HandleSwapping(0);
	}
	
	private void HandleSwapping(float currentRPM)
	{
		if(_blurred && _main && _smoothBlur && _hardBlur)
		{
			Debug.Log(currentRPM);
			if(currentRPM > _minQuadRPM && currentRPM < _minSwapRPM)
			{
				_blurred.SetActive(true);
				_main.SetActive(false);
				_propellerRenderer.material.SetTexture("_MainTex", _smoothBlur);
			}
			else if(currentRPM > _minSwapRPM)
			{
				_blurred.SetActive(true);
				_main.SetActive(false);
				_propellerRenderer.material.SetTexture("_MainTex", _hardBlur);
			}
			else
			{
				_blurred.SetActive(false);
				_main.SetActive(true);
			}
		}
	}
	
	public void HandlePropeller(float currentRPM)
	{
		float dps = ((currentRPM * 360) * 0.0166f) * Time.deltaTime;
		
		dps = Mathf.Clamp(dps, 25f, _minRotRPM);
		
		transform.Rotate(Vector3.forward, dps);
		
		HandleSwapping(currentRPM);
	}
}

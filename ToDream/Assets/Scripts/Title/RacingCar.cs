using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RacingCar : MonoBehaviour
{
	[Header("Cameras")]
	[SerializeField] private CinemachineVirtualCamera _backCam;
	[SerializeField] private CinemachineVirtualCamera _upperSideCam;
	
	[Header("Positions")]
	[SerializeField] private Transform _spawnPos;
	[SerializeField] private Transform _startPos;
	[SerializeField] private Transform _middle;
	[SerializeField] private Transform _endPos;
	
	[Header("Settings")]
	[SerializeField] private float _speed;
	
	private int _checkCar = 0;
	private bool _checkChange;
	
	protected void OnDisable()
	{
		this.transform.position = _spawnPos.position;
		_checkChange = false;
	}
	
	private void Update()
	{
		if(this.gameObject.activeSelf)
		{
			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
			
			if(this.transform.position.x > _startPos.position.x && _backCam.Follow == null && _checkCar == 0)
			{
				SetVirtualCam();
			
				_backCam.gameObject.SetActive(true);
			
				_checkCar = 1;
			}
			else if(this.transform.position.x > _middle.position.x && !_checkChange && _checkCar == 1)
			{
				ChangeCam(_backCam, _upperSideCam);
				_checkChange = true;
				_checkCar = 2;
			}
			else if(this.transform.position.x > _endPos.position.x && _checkCar == 2)
			{
				ReleaseVirtualCam();
			
				StartCoroutine("Restart");
			}
		}
	}
	
	private void ChangeCam(CinemachineVirtualCamera current, CinemachineVirtualCamera target)
	{
		if(current.gameObject.activeSelf)
		{
			current.gameObject.SetActive(false);
			target.gameObject.SetActive(true);
		}
		else
		{
			current.gameObject.SetActive(true);
			target.gameObject.SetActive(false);
		}
	}
	
	private void SetVirtualCam()
	{
		_backCam.LookAt = transform.GetChild(1);
		_backCam.Follow = transform.GetChild(1);
			
		_upperSideCam.LookAt = transform.GetChild(2);
		_upperSideCam.Follow = transform.GetChild(2);
	}
	
	private void ReleaseVirtualCam()
	{
		_backCam.gameObject.SetActive(false);
		_upperSideCam.gameObject.SetActive(false);
		
		_backCam.LookAt = null;
		_backCam.Follow = null;
			
		_upperSideCam.LookAt = null;
		_upperSideCam.Follow = null;
	}
	
	IEnumerator Restart()
	{
		yield return new WaitForSeconds(5f);
		this.transform.gameObject.SetActive(false);
		_checkCar = 0;
	}
}

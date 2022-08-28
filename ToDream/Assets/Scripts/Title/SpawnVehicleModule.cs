using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI Vehicles
public class SpawnVehicleModule : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject _vehicle;
	[SerializeField] private float _vehicleSpeed;
	[SerializeField] private Transform _startPos;
	[SerializeField] private Transform _endPos;
	
	private GameObject _currentVehicle;
	
	private void Start()
	{
		_currentVehicle = Instantiate(_vehicle, _startPos.position, _vehicle.transform.rotation);
		_currentVehicle.transform.SetParent(this.transform);
	}
	
	private void Update()
	{
		if(_currentVehicle.transform.localPosition.z > _endPos.localPosition.z)
		{
			_currentVehicle.SetActive(false);
			_currentVehicle.transform.position = _startPos.position;
			_currentVehicle.SetActive(true);
		}
		else
		{
			_currentVehicle.transform.Translate(Vector3.forward * _vehicleSpeed * Time.deltaTime);
		}
	}
	
	protected void OnDrawGizmos()
	{
		Debug.DrawLine(_startPos.position, _endPos.position, Color.red, 0.1f);
	}
}

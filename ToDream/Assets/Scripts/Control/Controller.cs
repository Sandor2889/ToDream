using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Controller : MonoBehaviour
{
	[SerializeField] private Controlable _controlTarget;
	[SerializeField] private CinemachineVirtualCamera _vCam;
	protected Controlable _ControlTarget
	{
		set { _controlTarget = value; } 
		get { return _controlTarget; } 
	}
	
	// 코드 너무 지저분함
	public void ChangeControlTarget(Controlable origin, Controlable target)
	{
		// 비행기는 x 축으로 (앞쪽이 위로) 기울어진 형태이다.
		if(origin._vehicleType != VehicleType.Air)
		{
			target.transform.rotation = origin.transform.rotation;
		}
		else
		{
			target.transform.rotation =
				new Quaternion(0, target.transform.rotation.y, target.transform.rotation.z, 0);
		}
		
		target.transform.position = origin.transform.position + new Vector3(0, 2f, 0);
		
		_vCam.Follow = target.transform.GetChild(target.transform.childCount - 1);
		_vCam.LookAt = target.transform.GetChild(target.transform.childCount - 1);;
		
		// 차는 Sphere 와 Collider 가 있기에 부모 설정이 필요하다.
		if(origin._vehicleType == VehicleType.Car)
		{
			VehicleControlable vehicle = origin as VehicleControlable;
			vehicle._MotorRB.transform.SetParent(vehicle.transform);
			vehicle._CarColliderRB.transform.SetParent(vehicle.transform);
		}
		
		origin.gameObject.SetActive(false);
		origin.transform.position = Vector3.zero;
		origin.transform.rotation = Quaternion.identity;
		
		target.gameObject.SetActive(true);
		
		_controlTarget = target;
	}
}

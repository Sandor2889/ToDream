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
	
	public void ChangeControlTarget(Controlable origin, Controlable target)
	{
		target.transform.eulerAngles = origin.transform.eulerAngles;
		target.transform.position = origin.transform.position + new Vector3(0, 2f, 0);
		
		_vCam.Follow = target.transform;
		_vCam.LookAt = target.transform;
		
		origin.gameObject.SetActive(false);
		origin.transform.position = Vector3.zero;
		origin.transform.rotation = Quaternion.identity;
		
		target.gameObject.SetActive(true);
		
		_controlTarget = target;
	}
}

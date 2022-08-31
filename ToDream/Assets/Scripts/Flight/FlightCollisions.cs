using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCollisions : MonoBehaviour
{
	PlayerController _pc;
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Water"))
		{
			if(_pc == null) _pc = FindObjectOfType<PlayerController>();
			Debug.Log("SS");
			Transform target = RespawnManager._Instance.Respawn();
			_pc._Current.transform.position = target.position;
			_pc._Current.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
			_pc.RespawnCharacter(transform.GetComponentInParent<FlightControlable>());
			
			
		}
	}
}

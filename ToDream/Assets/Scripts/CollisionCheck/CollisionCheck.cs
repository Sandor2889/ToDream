using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
	private float _startCount = 0;
	private float _count;
	private float _endCount = 5;
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			_count = _startCount;
			Debug.Log("Start" + _count);
		}
	}
	
	protected void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			_count += Time.fixedDeltaTime;
			if(_count >= _endCount)
			{
				Debug.Log("Respawn" + _count);
			}
		}
	}
	
	protected void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			_count = _startCount;
			Debug.Log("End" + _count);
		}
	}
}

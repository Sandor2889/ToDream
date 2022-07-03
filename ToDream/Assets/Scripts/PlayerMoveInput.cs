using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveInput : MonoBehaviour
{
	[SerializeField] private float _moveSpeed;
	private Vector3 _moveForce;
	
	public void MoveTo(Vector3 direction)
	{
		direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
		
		_moveForce = new Vector3(direction.x * _moveSpeed, _moveForce.y, direction.z * _moveSpeed);
	}
}

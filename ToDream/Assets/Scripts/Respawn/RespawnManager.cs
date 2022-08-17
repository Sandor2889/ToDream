using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	private static RespawnManager _instance = null;
	public static RespawnManager _Instance
	{
		get {return _instance;}
	}

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
	public List<Transform> _lstRespawnTr;
	private Transform _target;
	private float _distance;
	
	public Transform Respawn(Transform playerPos)
	{
		_target = null;
		_distance = 10000000;
		foreach(Transform pos in _lstRespawnTr)
		{
			float dis = Vector3.Distance(playerPos.position, pos.position);
			
			if(_distance > dis)
			{
				_distance = dis;
				_target = pos;
				Debug.Log(_target.name + " , " + _distance);
			}
		}
		
		return _target;
	}
}

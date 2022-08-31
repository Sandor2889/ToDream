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

    
	[SerializeField] private Transform _target;
	
	
	public Transform Respawn()
	{
		return _target;
	}
}

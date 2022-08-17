using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager _Instance
	{
		get {return _instance;}
		set {_instance = FindObjectOfType<GameManager>();}
	}
	
	public Controlable _player;
	
	public Controlable GetPlayer()
	{
		return _player;
	}
}

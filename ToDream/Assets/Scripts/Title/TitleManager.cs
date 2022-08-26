using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleManager : MonoBehaviour
{
	public GameObject _car;
	
	private void Start()
    {
	    Application.targetFrameRate = 60;
    }
    
	private void Update()
	{
		if(!_car.activeSelf)
		{
			_car.SetActive(true);
		}
	}
}

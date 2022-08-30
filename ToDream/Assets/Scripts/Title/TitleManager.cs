using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	
	public void ChangeScene(string sceneName)
	{
		LevelManager._Instance.LoadScene(sceneName);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}

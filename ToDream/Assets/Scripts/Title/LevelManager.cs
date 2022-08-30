using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	private static LevelManager _instance;
	public static LevelManager _Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<LevelManager>();
			}
			return _instance;
		}
	}
	
	[SerializeField] private GameObject _loaderPanel;
	[SerializeField] private Image _bar;
	
	private void Awake()
	{
		if(_instance == null)
		{
			_instance = this;
			//DontDestroyOnLoad(gameObject);
		}
		//else
		//{
		//	Destroy(gameObject);
		//}
	}
	
	public async void LoadScene(string sceneName)
	{
		var scene = SceneManager.LoadSceneAsync(sceneName);
		scene.allowSceneActivation = false;
		
		_loaderPanel.SetActive(true);
		
		do
		{
			await System.Threading.Tasks.Task.Delay(100);

			_bar.fillAmount = scene.progress;

		} while(scene.progress < 0.9f);

		await System.Threading.Tasks.Task.Delay(1000);

		scene.allowSceneActivation = true;
		_loaderPanel.SetActive(false);
	}
}

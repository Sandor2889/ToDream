using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string _name;
    public AudioClip _clip;
}


public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    Debug.Log("Did not find AudioManager");
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField] private AudioSource _inGameBgmPlayer;
    [SerializeField] private AudioSource _sfxComplete;

    public AudioSource _SfxComplete => _sfxComplete;

    public void Awake()
    {
        _instance = this;
    }

    public void PlayAduio(AudioSource source)
    {
        source.Play();
    }

   
}

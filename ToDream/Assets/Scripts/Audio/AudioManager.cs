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

    [SerializeField] private Sound _titleBgm;
    [SerializeField] private Sound _inGameBgm;

    [SerializeField] private AudioSource _titleBgmPlayer;
    [SerializeField] private AudioSource _inGameBgmPlayer;

    public void Awake()
    {
        _instance = this;
    }

    public void PlayTitleBGM(string strBgm)
    {
        if (strBgm.GetHashCode() == _titleBgm._name.GetHashCode())
        {
            _titleBgmPlayer.clip = _titleBgm._clip;
            _titleBgmPlayer.Play();
        }
    }

    public void PlayInGameBGM(string strBgm)
    {
        if (strBgm.GetHashCode() == _inGameBgm._name.GetHashCode())
        {
            _inGameBgmPlayer.clip = _inGameBgm._clip;
            _inGameBgmPlayer.Play();
        }
    }

    public void StopBGM()
    {
        _inGameBgmPlayer.Stop();
    }
}

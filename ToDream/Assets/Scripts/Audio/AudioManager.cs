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

    [SerializeField] private Sound[] _sfx;
    [SerializeField] private Sound[] _bgm;

    [SerializeField] private AudioSource _bgmPlayer;
    [SerializeField] private AudioSource[] _sfxPlayer;

    public void Awake()
    {
        _instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            PlayBGM("DelightFul");
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayBGM("PlayGroundFun");
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayBGM("GamesWorldbeat");
        }
    }

    public void PlayBGM(string strBgm)
    {
        for (int i = 0; i < _bgm.Length; i++)
        {
            if (strBgm.GetHashCode() == _bgm[i]._name.GetHashCode())
            {
                _bgmPlayer.clip = _bgm[i]._clip;
                _bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    public void PlaySFX(string strSfx)
    {
        for (int i = 0; i < _sfx.Length; i++)
        {
            if (strSfx.GetHashCode() == _sfx[i]._name.GetHashCode())
            {
                for(int j = 0; j < _sfxPlayer.Length; i++)
                {
                    if (!_sfxPlayer[j].isPlaying)
                    {
                        _sfxPlayer[j].clip = _sfx[i]._clip;
                        _sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }

        Debug.Log(strSfx + " - 이름의 효과음이 없습니다.");
    }
}

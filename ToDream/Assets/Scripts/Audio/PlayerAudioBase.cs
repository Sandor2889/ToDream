using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAudioBase : MonoBehaviour
{
    [SerializeField] protected Sound[] _sfx;
    [SerializeField] protected AudioSource _sfxPlayer;

    protected virtual void Awake()
    {
        _sfxPlayer = GetComponent<AudioSource>();
    }

    public abstract void PlaySFX(string strSFX);

    //protected abstract void StopSFX();
}

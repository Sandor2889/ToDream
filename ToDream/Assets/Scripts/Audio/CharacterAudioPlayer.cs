using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioPlayer : PlayerAudioBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void PlaySFX(string strSFX)
    {
        for (int i = 0; i < _sfx.Length; i++)
        {
            if (strSFX.GetHashCode() == _sfx[i]._name.GetHashCode())
            {
                _sfxPlayer.clip = _sfx[i]._clip;
                _sfxPlayer.Play();
            }
        }
    }
}

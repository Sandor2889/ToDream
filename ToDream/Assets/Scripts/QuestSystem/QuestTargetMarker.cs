using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTargetMarker : MonoBehaviour
{
    private ParticleSystem _effect; 
    public string key;

    private void Awake()
    {
        _effect = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _effect.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager._Instance.ReceiveReport(key, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region Singleton
    private static WaveManager _instance;
    public static WaveManager _Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<WaveManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    Debug.LogError("Instance is duplicated");
                }
            }
            return _instance;
        }
    }
    #endregion

    public float _amplitude = 1f;   // 진폭
    public float _length = 2f; // 파장
    public float _speed = 1f; // 주기
    private float _offset = 0f;


    private void Update()
    {
        _offset += Time.deltaTime * _speed;
    }

    // 싱글톤 메소드 호출 가독성을 위한 작업
    private float _GetWaveHeight(float x)
    {
        return _amplitude * Mathf.Sin(x / _length + _offset);
    }

    // 필요시 이 메소드를 호출하면 된다.
    public static float GetWaveHeight(float x)
    {
        return _Instance._GetWaveHeight(x);
    }
}
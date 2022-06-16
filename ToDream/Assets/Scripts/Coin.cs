using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Text coinText;

    private float Coins { get; set; }
    private int coin;

    private void Awake()
    {
        coinText = GetComponent<Text>();
    }

    private void Update()
    {
        IncreaseCoin();
    }


    // 단위 시간당 코인 증가량
    public void IncreaseCoin()
    {
        Coins += Time.deltaTime;
        coin = (int)Coins;
        coinText.text = coin.ToString();
    }
}

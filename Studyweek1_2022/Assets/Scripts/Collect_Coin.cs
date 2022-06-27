using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collect_Coin : MonoBehaviour
{       
    public UICoinCounter _UICoinCounter;
    [SerializeField] Text _CoinText;

    private void Awake()
    {
        _UICoinCounter.CoinCounter = 0;
        _CoinText.text = _UICoinCounter.CoinCounter.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            _UICoinCounter.CoinCounter++;
           // StartCoroutine(CoinCooldown());
            
            Debug.Log($"Coins: { _UICoinCounter.CoinCounter}");

            _CoinText.text = _UICoinCounter.CoinCounter.ToString();          
        }
    }
}

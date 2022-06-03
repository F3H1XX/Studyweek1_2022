using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collect_Coin : MonoBehaviour
{
    
    public Text CoinCountText;
    public  UICoinCounter _UICoinCounter;
    

    private void Awake()
    {
        CoinCountText = GetComponent<Text>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            _UICoinCounter.CoinCounter++;
           // StartCoroutine(CoinCooldown());
            
            Debug.Log($"Coins: { _UICoinCounter.CoinCounter}");
            
        }
    }

   /* public IEnumerator CoinCooldown()
    {
        _UICoinCounter.CoinCounter++;
        yield return new WaitForSeconds(0.1f);
    } */
}

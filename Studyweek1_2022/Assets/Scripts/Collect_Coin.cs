using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Coin : MonoBehaviour
{
    public static int coinCounter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            coinCounter++;
            Debug.Log($"Coins: { coinCounter.ToString()}");
        }
            
    }
}

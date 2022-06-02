using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Hover : MonoBehaviour
{
    private Rigidbody2D coinRB;
    private Vector2 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
        coinRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + Vector2.down * Mathf.Sin(Time.time);
        coinRB.AddTorque(1, ForceMode2D.Force);
    }
}

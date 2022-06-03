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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + Vector2.down * Mathf.Sin(Time.time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Rigidbody2D enemyRigidbody;
    [SerializeField] BoxCollider2D groundDetectionCollider;

    void Start() 
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // set enemyRidigbody zu Rigidbody2D um mit diesem arbeiten zu k�nnen
    }


    void Update()
    {
        if (isFacingRight())
        {
            //move right
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            //move left
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private bool isFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; // Epsilon schau nach einem sehr kleinen float Wert (0.000001f)
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Dreht den Enemy
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), transform.localScale.y);
    }

}
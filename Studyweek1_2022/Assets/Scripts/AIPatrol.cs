using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;
    public Transform groundCheckPos;
    public float walkSpeed;
    public LayerMask Ground;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
    }

    void fixedUpdate()
    {
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, Ground);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
    }

    void Patrol() //Bewegt den Enemy in Richtung Vector.Y
    {
        if (mustTurn)
        {
            Flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip() //Dreht den Enemy in der Laufrichtung um
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}

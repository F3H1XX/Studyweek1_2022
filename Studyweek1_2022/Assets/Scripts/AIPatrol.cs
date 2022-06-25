using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int enemyScoreValue;
    [SerializeField] Rigidbody2D enemyRigidbody;
    [SerializeField] BoxCollider2D groundDetectionCollider;
    [SerializeField] GameObject body;
    [SerializeField] GameObject groundCollider;
    [SerializeField] float hitBounceForce;
    [SerializeField] Transform ObstacleDetector;
    [SerializeField] LayerMask obstacles;
    public UICoinCounter _UICoinCounter;
    [SerializeField] Text _CoinText;

    Animator _animator;
    
	

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>(); // set enemyRidigbody zu Rigidbody2D um mit diesem arbeiten zu k�nnen
        _animator = GetComponent<Animator>();    
		_animator.enabled = true;
		_animator.SetBool("IsWalking", true);
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
        // damageCheck();
        ObstacleCheck();
    }

    private bool isFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; // Epsilon schaut nach einem sehr kleinen float Wert (0.000001f)
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Dreht den Enemy
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), transform.localScale.y);
    }
    public IEnumerator DeathAnimationCooldownEnemy()
    { 
        yield return new WaitForSeconds(0.9f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.attachedRigidbody.AddForce(new Vector2(0, hitBounceForce), ForceMode2D.Impulse);
            GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            groundCollider.GetComponent<Collider2D>().enabled = false;
            moveSpeed = 0f;
            transform.gameObject.tag = "Untagged";           
            body.SetActive(false);
            _animator.SetBool("Die", true);
            StartCoroutine("DeathAnimationCooldownEnemy");
            _UICoinCounter.CoinCounter += enemyScoreValue;
            _CoinText.text = _UICoinCounter.CoinCounter.ToString();
        }
    }
       
    private void ObstacleCheck()
    {
        Collider2D[] Obstacles = Physics2D.OverlapCircleAll(ObstacleDetector.position, 0.3f, obstacles);

        if(Obstacles.Length != 0)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), transform.localScale.y);
        }
    }
}
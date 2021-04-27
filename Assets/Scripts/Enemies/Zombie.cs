using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    [SerializeField] float moveSpeed = 3f;
    [SerializeField] int health = 2;

    Rigidbody2D zombieRigidbody;
    CapsuleCollider2D zombieCollider;
    Animator zombieAnimator;
    Player player;

    bool hasCollided = false;
    bool isAlive = true;
    bool knockback = false;

    // Start is called before the first frame update
    void Start()
    {
        zombieRigidbody = GetComponent<Rigidbody2D>();
        zombieCollider = GetComponent<CapsuleCollider2D>();
        zombieAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        hasCollided = false;
        if (isAlive && !knockback){
            
            Walk();
        }
        
    }

    private void Walk()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        zombieRigidbody.velocity = new Vector2(moveSpeed*direction, zombieRigidbody.velocity.y);
    }

    private void TakeDamage(int damageTaken, Collider2D collision)
    {
        if (knockback)
        {
            return;
        }
        health -= damageTaken;
        knockback = true;
        if (health > 0)
        {
            float direction = Mathf.Sign(transform.position.x - collision.gameObject.transform.position.x);
            zombieAnimator.SetBool("Damaged", true);
            zombieRigidbody.velocity = new Vector2(5f * direction, 3f);
        }
        else
        {
            isAlive = false;
            zombieRigidbody.velocity = new Vector2(0f, 0f);
            zombieAnimator.SetBool("Dying", true);
            zombieCollider.enabled = false;
            zombieRigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void UnsetDamaged()
    {
        zombieAnimator.SetBool("Damaged", false);
    }

    public void UnsetDying()
    {
        zombieAnimator.SetBool("Dying", false);
    }
    
    public void UnsetKnockback()
    {
        knockback = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if((!collision.CompareTag("Sword Attack")) && (!collision.CompareTag("Eldritch Blast")) && (!collision.CompareTag("Player")))
        {
            transform.localScale = new Vector2(-transform.localScale.x, 1f);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided && player.GetAlive() && isAlive)
        {
            player.TakeDamage(1);
            hasCollided = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword Attack") && !hasCollided)
        {
            hasCollided = true;
            TakeDamage(1, collision);
        }
        if (collision.CompareTag("Eldritch Blast") && !hasCollided && collision.isTrigger)
        {
            hasCollided = true;
            TakeDamage(2, collision);
        }
    }
}

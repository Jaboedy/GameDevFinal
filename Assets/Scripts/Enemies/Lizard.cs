using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    [SerializeField] float walkSpeed = -2f;
    [SerializeField] int health = 3;
    
    Animator lizardAnimator;
    Rigidbody2D lizardRigidBody;
    CapsuleCollider2D lizardBodyCollider;
    
    bool isAttacking = false;
    bool isAlive = true;
    bool knockback = false;
    bool hasCollided = false;

    

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        lizardAnimator = GetComponent<Animator>();
        lizardRigidBody = GetComponent<Rigidbody2D>();
        lizardBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!isAttacking) && (!knockback) && (isAlive))
        {
            Walk();
        }
        if (isAlive)
        {
            lizardAnimator.SetBool("Attacking", isAttacking);
            hasCollided = false;
        }
        
    }

    public void Attack()
    {
        if (isAlive)
        {
            isAttacking = true;
            lizardRigidBody.velocity = new Vector2(0f, 0f);
            lizardAnimator.SetBool("Attacking", true);
        }
    }

    private void Walk()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        lizardRigidBody.velocity = new Vector2(walkSpeed * direction, lizardRigidBody.velocity.y);
    }

    public void DoneAttacking()
    {
        isAttacking = false;
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
            lizardAnimator.SetBool("Damaged", true);
            lizardRigidBody.velocity = new Vector2(5f * direction, 3f);
        }
        else
        {
            isAlive = false;
            lizardAnimator.SetBool("Dying", true);
            lizardBodyCollider.enabled = false;
            lizardRigidBody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void UnsetDamaged()
    {
        lizardAnimator.SetBool("Damaged", false);
    }

    public void UnsetDying()
    {
        lizardAnimator.SetBool("Dying", false);
    }

    public void UnsetKnockback()
    {
        knockback = false;
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

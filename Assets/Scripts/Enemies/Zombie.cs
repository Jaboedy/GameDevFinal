using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    [SerializeField] float moveSpeed = 3f;
    [SerializeField] int health = 2;

    Rigidbody2D zombieRigidbody;
    Collider2D zombieCollider;
    Player player;

    bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        zombieRigidbody = GetComponent<Rigidbody2D>();
        zombieCollider = GetComponent<Collider2D>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        hasCollided = false;
        Walk();
    }

    private void Walk()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        zombieRigidbody.velocity = new Vector2(moveSpeed*direction, zombieRigidbody.velocity.y);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-transform.localScale.x, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided && player.GetAlive())
        {
            player.TakeDamage(1);
            hasCollided = true;
        }
    }
}

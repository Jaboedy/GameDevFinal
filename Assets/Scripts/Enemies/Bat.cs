using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{

    [SerializeField] float flySpeed;

    Player player;
    bool hasCollided = false;
    bool isAlive = true;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hasCollided = false;
        if (isAlive && player.GetAlive())
        {
            Fly();
            FlipSprite();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasCollided && player.GetAlive())
        {
            player.TakeDamage(1);
            hasCollided = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword Attack"))
        {
            myAnimator.SetBool("Dying", true);
            myCollider.enabled = false;
            myRigidbody.velocity = new Vector2(0f, 0f);
            isAlive = false;
            player.AddMana();
        }
        if (collision.CompareTag("Eldritch Blast"))
        {
            myAnimator.SetBool("Dying", true);
            myCollider.enabled = false;
            myRigidbody.velocity = new Vector2(0f, 0f);
            isAlive = false;
        }
    }

    private void Fly()
    {
        float xDirSign = Mathf.Sign(player.transform.position.x - transform.position.x);
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y+0.6f);
        float yDirSign = Mathf.Sign(direction.y);
        LayerMask layers = LayerMask.GetMask("Ground", "Player");
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, Mathf.Infinity, layers);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                myRigidbody.velocity = direction.normalized * flySpeed;
            }
            else
            {
                RaycastHit2D ceiling = Physics2D.Raycast(startPos, Vector2.up, Mathf.Infinity, layers);
                RaycastHit2D floor = Physics2D.Raycast(startPos, Vector2.down, Mathf.Infinity, layers);
                if (ceiling.distance < floor.distance)
                {
                    myRigidbody.velocity = new Vector2(0.7f * flySpeed * xDirSign, -0.7f * flySpeed);
                }
                else
                {
                    myRigidbody.velocity = new Vector2(0.7f * flySpeed * xDirSign, 0.7f * flySpeed);
                }
                    
            }
        }
        
        
    }

    private void FlipSprite()
    {
        float xDirection = Mathf.Sign(myRigidbody.velocity.x);
        bool isTurning = (xDirection == transform.localScale.x);
        if (isTurning)
        {
            transform.localScale = new Vector2(-xDirection, 1f);
        }
    }

    public void DestroyAfterAnimation()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void UnsetDying()
    {
        myAnimator.SetBool("Dying", false);
    }
}

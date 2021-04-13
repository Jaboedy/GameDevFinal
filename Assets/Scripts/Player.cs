using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] int health = 5;

    //states
    bool isAlive = true;
    
    //cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("Attacking", false);
        Run();
        Jump();
        Fall();
        Attack();
        FlipSprite();
        
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAnimator.SetBool("Attacking", true);
        }
    }

    private void Run()
    {
        float xMove = Input.GetAxis("Horizontal") * runSpeed;
        Vector2 playerVelocity = new Vector2(xMove, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasXSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasXSpeed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("Running", true);
        }
        else
        {
            myAnimator.SetBool("Running", false);
        }
    }

    private void Jump()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("Jumping", false);
            return;
        }
        if (Input.GetKeyDown("space"))
        {
            Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
            myRigidBody.velocity = playerVelocity;
            myAnimator.SetBool("Jumping", true);
        }
    }

    private void Fall()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("Falling", false);
            return;
        }

        bool playerIsFalling = Mathf.Sign(myRigidBody.velocity.y) < Mathf.Epsilon;
        Debug.Log(myRigidBody.velocity.y);
        if (playerIsFalling)
        {
            myAnimator.SetBool("Falling", true);
        }
    }

    private void FlipSprite()
    {
        bool playerHasXSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        float direction = Mathf.Sign(myRigidBody.velocity.x);
        bool isTurning = (direction != transform.localScale.x);
        if (playerHasXSpeed)
        {
            transform.localScale = new Vector2(direction, 1f);
            if (isTurning)
            {
                transform.position = new Vector3(transform.position.x + (.5f * direction), transform.position.y, transform.position.z);
            }
        }
        
    }
}

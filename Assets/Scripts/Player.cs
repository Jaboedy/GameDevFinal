using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;

    //states
    bool isAlive = true;
    
    //cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("Jumping", false);
        Run();
        Jump();
        FlipSprite();
        
    }

    private void Run()
    {
        float xMove = Input.GetAxis("Horizontal") * runSpeed;
        Vector2 playerVelocity = new Vector2(xMove, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasXSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasXSpeed)
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
        if (Input.GetKeyDown("space"))
        {
            Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, jumpHeight);
            myRigidBody.velocity = playerVelocity;
            myAnimator.SetBool("Jumping", true);
        }
    }

    private void FlipSprite()
    {
        bool playerHasXSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasXSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

}

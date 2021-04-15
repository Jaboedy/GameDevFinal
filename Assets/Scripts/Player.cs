using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] int health = 5;
    [SerializeField] int mana = 3;

    //states
    [SerializeField] bool isAlive = true; //remove serialize field when done testing
    int spawnID = 0;
    
    //cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;


    private void Awake()
    {
        int playerCount = FindObjectsOfType<Player>().Length;
        if(playerCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
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
        
        if (isAlive)
        {
            myAnimator.SetBool("Attacking", false);
            Run();
            Jump();
            Fall();
            Attack();
            DamageTest();
            FlipSprite();
        }
    }

    private void DamageTest()
    {
        if (Input.GetKeyDown("j"))
        {
            TakeDamage(2);
        }
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        //insert something to update canvas
        if (health < 1)
        {
            isAlive = false;
            myAnimator.SetBool("Attacking", false);
            myAnimator.SetBool("Dying", true);
        }
    }

    public void SetSpawnID(int id)
    {
        spawnID = id;
    }

    public int GetSpawnID()
    {
        return spawnID;
    }

    public void UnsetDying()
    {
        myAnimator.SetBool("Dying", false);
    }
}

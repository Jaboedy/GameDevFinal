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
    [SerializeField] float boostSpeed = 10f;
    [SerializeField] UIController uiController;
    [SerializeField] EldritchBlast eldritchBlastPrefab;
    [SerializeField] SwordAudio sword;
    [SerializeField] AudioClip[] playerSounds;

    //states
    bool isAlive = true; //remove serialize field when done testing
    bool knockback = false;
    int spawnID = 0;
    bool paused = false;
    
    //cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    [SerializeField] EldritchBlast eldritchBlast;

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
        
        if ((isAlive) && (!knockback) && (!paused))
        {
            myAnimator.SetBool("Attacking", false);
            Run();
            Jump();
            Fall();
            Attack();
            Cast();
            FlipSprite();
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAnimator.SetBool("Attacking", true);
        }
    }

    private void Cast()
    {
        if ((Input.GetMouseButtonDown(1)) && (!myAnimator.GetBool("Attacking")) && (mana>0) && (!eldritchBlast))
        {
            knockback = true;
            myRigidBody.velocity = new Vector2(0f, 0f);
            myAnimator.SetBool("Casting", true);
            myRigidBody.gravityScale = 0;
            mana--;
            uiController.SetMana(mana);
            
        }
        else if ((Input.GetMouseButtonDown(1)) && (eldritchBlast))
        {
            eldritchBlast.Explode();
        }
    }

    private void SpawnEldritchBlast()
    {
        Vector3 eldritchSpawnPos = new Vector3(transform.position.x + (.7f * transform.localScale.x), transform.position.y + 0.6f, transform.position.z);
        eldritchBlast = Instantiate(eldritchBlastPrefab, eldritchSpawnPos, transform.rotation);
    }

    private void FinishCast()
    {
        myAnimator.SetBool("Casting", false);
        knockback = false;
        myRigidBody.gravityScale = 1;
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
        if (health < 0)
        {
            health = 0;
        }

        uiController.SetHP(health);
        //insert something to update canvas
        if (health < 1)
        {
            isAlive = false;
            myAnimator.SetBool("Attacking", false);
            myAnimator.SetBool("Dying", true);
        }
        else
        {
            myAnimator.SetBool("Damaged", true);
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

    public void SetKnockback()
    {
        knockback = true;
    }

    public void UnsetKnockback()
    {
        knockback = false;
        myAnimator.SetBool("Damaged", false);
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    public void StopPlayer()
    {
        myRigidBody.velocity = new Vector2(0f, 0f);
    }

    public int GetHP()
    {
        return health;
    }

    public int GetMana()
    {
        return mana;
    }

    public void AddMana()
    {
        if (mana < 3)
        {
            mana += 1;
            uiController.SetMana(mana);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !knockback)
        {
            knockback = true;
            float direction = Mathf.Sign(transform.position.x - collision.gameObject.transform.position.x);
            myRigidBody.velocity = new Vector2(10f * direction, 3f);
            myAnimator.SetBool("Damaged", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Eldritch Blast"))
        {
            StartCoroutine("Boost", collision);
        }
    }

    IEnumerator Boost(Collider2D collision)
    {
        knockback = true;

        Vector2 direction = new Vector2(transform.position.x - collision.transform.position.x, transform.position.y - collision.transform.position.y);
        myRigidBody.velocity = direction.normalized * boostSpeed;
        while(myRigidBody.velocity.y > -2f)
        {
            yield return new WaitForSeconds(.1f);
        }
        knockback = false;
        
    }
    public void SetPaused(bool pausedBool)
    {
        paused = pausedBool;
    }

    public void WalkSound()
    {
        if (playerSounds.Length > 0)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            AudioSource.PlayClipAtPoint(playerSounds[0], point + new Vector3(0.05f*transform.localScale.x, 0, 0), 0.2f);
        }
    }

    public void AttackSound()
    {
        sword.SwingAudio();
    }

    public void DestroyPlayer()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

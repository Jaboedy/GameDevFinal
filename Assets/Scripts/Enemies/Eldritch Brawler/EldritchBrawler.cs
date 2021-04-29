using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldritchBrawler : MonoBehaviour
{

    [SerializeField] GameObject leftWaypoint;
    [SerializeField] GameObject centerWaypoint;
    [SerializeField] GameObject rightWaypoint;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float idleDistance = 1f;
    [SerializeField] int health = 20;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;

    Player player;
    Animator brawlerAnimator;
    Rigidbody2D brawlerRigidbody;

    BossRoomController bossRoomController;


    bool isMovingToWaypoint = false;
    bool isAttacking = false;
    bool waitingToFlip = false;
    bool hasCollided = false;
    bool isAlive = true;
    bool hasTakenDamage = false;
    bool canMove = true;
    bool scriptedAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        bossRoomController = FindObjectOfType<BossRoomController>();
        brawlerAnimator = GetComponent<Animator>();
        brawlerRigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine("WaypointCheck");
    }

    // Update is called once per frame
    void Update()
    {
        if ((isAlive) && (!scriptedAttack))
        {
            hasCollided = false;
            if (!isAttacking && !waitingToFlip && canMove)
            {
                Walk();
            }
            FlipSprite();
        }
    }

    public void PoundAttack()
    {
        isAttacking = true;
        brawlerAnimator.SetBool("Pounding", true);
        brawlerRigidbody.bodyType = RigidbodyType2D.Static;
    }

    public void EndPounding()
    {
        brawlerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        brawlerAnimator.SetBool("Pounding", false);
        isAttacking = false;
    }

    public void Walk()
    {
        float direction;
        
        if (isMovingToWaypoint)
        {
            direction = Mathf.Sign(centerWaypoint.transform.position.x - gameObject.transform.position.x);
            brawlerRigidbody.velocity = new Vector2(walkSpeed * direction, brawlerRigidbody.velocity.y);
            brawlerAnimator.SetBool("Walking", true);
        }
        else
        {
            direction = Mathf.Sign(player.transform.position.x - gameObject.transform.position.x);
            float xDist = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
            if (xDist > idleDistance)
            {
                brawlerRigidbody.velocity = new Vector2(walkSpeed * direction, brawlerRigidbody.velocity.y);
                brawlerAnimator.SetBool("Walking", true);
            }
            else
            {
                brawlerRigidbody.velocity = new Vector2(0f, brawlerRigidbody.velocity.y);
                brawlerAnimator.SetBool("Walking", false);
            }
        }
        
        

    }

    public void FlipSprite()
    {
        float xDist = Mathf.Abs(player.transform.position.x - transform.position.x);
        bool brawlerIsMoving = (Mathf.Abs(brawlerRigidbody.velocity.x) > 0);
        if ((xDist < idleDistance) && !waitingToFlip)
        {
            StartCoroutine("WaitToFlip");
        }
        else if (brawlerIsMoving && !waitingToFlip)
        {

            transform.localScale = new Vector2(Mathf.Sign(brawlerRigidbody.velocity.x), 1f);
        }
    }

    IEnumerator ScriptedAttack()
    {
        scriptedAttack = true;
        transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
        PoundAttack();
        while (isAttacking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        transform.localScale = new Vector2(-1 *transform.localScale.x, transform.localScale.y);
        yield return new WaitForSeconds(0.1f);
        PoundAttack();
        while (isAttacking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
        yield return new WaitForSeconds(0.1f);
        PoundAttack();
        scriptedAttack = false;
    }
    IEnumerator WaitToFlip()
    {
        waitingToFlip = true;
        brawlerAnimator.SetBool("Walking", false);
        while (isAttacking)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(.2f);
        transform.localScale = new Vector2(Mathf.Sign(brawlerRigidbody.velocity.x), 1f);
        waitingToFlip = false;
    }

    IEnumerator InvulnerabilityFrames()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        hasTakenDamage = false;
    }

    IEnumerator WaypointCheck()
    {
        while (isAlive)
        {
            if (!isMovingToWaypoint)
            {
                if (((transform.position.x < leftWaypoint.transform.position.x) || (transform.position.x > rightWaypoint.transform.position.x)) && hasTakenDamage)
                {
                    isMovingToWaypoint = true;
                }
            }
            else
            {
                if(Mathf.Abs(transform.position.x - centerWaypoint.transform.position.x) < 0.7f)
                {
                    isMovingToWaypoint = false;
                    StartCoroutine("ScriptedAttack");
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void MoveToWaypoint(GameObject brawlerWaypoint)
    {
        isMovingToWaypoint = true;
    }

    public bool GetMovingToWaypoint()
    {
        return isMovingToWaypoint;
    }

    public bool GetAlive()
    {
        return isAlive;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.GetAlive() && isAlive && !hasCollided)
        {
            player.TakeDamage(1);
            hasCollided = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Sword Attack") && !hasTakenDamage)
        {
            hasTakenDamage = true;
            StartCoroutine("InvulnerabilityFrames");
            if (health == 1)
            {
                player.AddMana();
            }
            TakeDamage(1, collision);

        }
        if (collision.CompareTag("Eldritch Blast") && !hasTakenDamage && collision.isTrigger)
        {
            hasTakenDamage = true;
            StartCoroutine("InvulnerabilityFrames");
            TakeDamage(2, collision);
        }
    }

    private void TakeDamage(int damageTaken, Collider2D collision)
    {
        
        health -= damageTaken;
        if (health > 0)
        {
            if (!isAttacking)
            {
                brawlerAnimator.SetBool("Hurting", true);
                brawlerRigidbody.velocity = new Vector2(0f, 0f);
                canMove = false;
            }
        }
        else
        {
            isAlive = false;
            brawlerAnimator.SetBool("Dying", true);
            brawlerRigidbody.bodyType = RigidbodyType2D.Static;
            body.GetComponent<Collider2D>().enabled = false;
            head.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void UnsetHurting()
    {
        brawlerAnimator.SetBool("Hurting", false);
        canMove = true;
    }

    public void FinishDying()
    {
        bossRoomController.Win();
    }
}

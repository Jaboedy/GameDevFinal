using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldritchBlast : MonoBehaviour
{
    private bool hasCollided = false;
    [SerializeField] GameObject collidedWith;
    [SerializeField] Vector2 collisionVector;


    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CircleCollider2D myCollider;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
        player = FindObjectOfType<Player>();
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myRigidBody.velocity = new Vector2(10f * player.transform.localScale.x, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        StuckCheck();
        Stuck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided)
        {
            Debug.Log(collision.gameObject.name);
            myRigidBody.velocity = new Vector2(0f, 0f);
            hasCollided = true;
            collidedWith = collision.gameObject;
            collisionVector = collision.gameObject.transform.position - gameObject.transform.position;
            myCollider.enabled = false;
        }
        
    }

    private void Stuck()
    {
        if (hasCollided && collidedWith != null)
        {
            
            gameObject.transform.position = new Vector2(collidedWith.transform.position.x - collisionVector.x, collidedWith.transform.position.y - collisionVector.y);
        }
    }

    private void StuckCheck()
    {
        if (hasCollided && collidedWith == null)
        {
            Destroy(gameObject);
        }
    }


    public void Explode()
    {
        myCollider.enabled = true;
        myRigidBody.velocity = Vector2.zero;
        myAnimator.SetBool("Explode", true);
    }
    public void FinishExplosion()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }


}

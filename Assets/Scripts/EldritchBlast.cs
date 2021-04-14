using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldritchBlast : MonoBehaviour
{
    private bool hasCollided = false;
    [SerializeField] GameObject collidedWith;
    [SerializeField] Vector2 collisionVector;


    Rigidbody2D myRigidBody;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
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
            myRigidBody.velocity = new Vector2(0f, 0f);
            hasCollided = true;
            collidedWith = collision.gameObject;
            collisionVector = collision.gameObject.transform.position - gameObject.transform.position;
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


}

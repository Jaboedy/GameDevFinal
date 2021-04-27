using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{

    GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((!collision.CompareTag("Sword Attack")) && (!collision.CompareTag("Eldritch Blast")) && (!collision.CompareTag("Player")))
        {
            parent.transform.localScale = new Vector2(-parent.transform.localScale.x, 1f);
        }
    }
}

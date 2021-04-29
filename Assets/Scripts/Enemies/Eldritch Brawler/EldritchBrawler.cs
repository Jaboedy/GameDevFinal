using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldritchBrawler : MonoBehaviour
{

    [SerializeField] GameObject[] bralwerWaypoints;

    Player player;
    Animator brawlerAnimator;
    Rigidbody2D brawlerRigidbody;

    bool movingToWaypoint = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        brawlerAnimator = GetComponent<Animator>();
        brawlerRigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PoundAttack()
    {
        brawlerAnimator.SetBool("Pounding", true);
    }

    public void EndPounding()
    {
        brawlerAnimator.SetBool("Pounding", false);
    }
}

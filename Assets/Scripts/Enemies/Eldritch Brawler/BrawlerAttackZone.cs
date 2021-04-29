using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerAttackZone : MonoBehaviour
{
    [SerializeField] EldritchBrawler brawler;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.GetAlive() && !brawler.GetMovingToWaypoint())
        {
            brawler.PoundAttack();
        }
    }
}

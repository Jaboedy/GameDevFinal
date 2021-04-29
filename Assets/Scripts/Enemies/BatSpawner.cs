using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{

    [SerializeField] Bat batPrefab;
    [SerializeField] EldritchBrawler brawler;
    Bat spawnedBat;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CheckToSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnBat()
    {
        spawnedBat = Instantiate(batPrefab, transform.position, transform.rotation);
    }

    IEnumerator CheckToSpawn()
    {
        while (brawler.GetAlive())
        {
            if (spawnedBat)
            {
                yield return new WaitForSeconds(4f);
            }
            else
            {
                SpawnBat();
            }
        }
    }
}

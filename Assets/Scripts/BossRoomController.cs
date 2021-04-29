using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BatSpawner[] batSpawners;
    [SerializeField] EldritchBrawler eldritchBrawler;

    BoxCollider2D controllerTrigger;
    void Start()
    {
        controllerTrigger = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controllerTrigger.enabled = false;
        eldritchBrawler.gameObject.SetActive(true);
        foreach(BatSpawner batSpawner in batSpawners)
        {
            batSpawner.gameObject.SetActive(true);
        }
    }

    public void Win()
    {
        StartCoroutine("WinCoroutine");
    }
    IEnumerator WinCoroutine()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}

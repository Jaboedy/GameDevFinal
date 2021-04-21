using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    
    [SerializeField] int exitID;
    [SerializeField] int nextSceneID;

    SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNextSpawnID()
    {
        sceneController.SetSpawnID(exitID);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sceneController.SetSpawnID(exitID);
            sceneController.LoadNextScene(nextSceneID);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    Player player;
    UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        uiController = FindObjectOfType<UIController>();
        player.DestroyPlayer();
        uiController.DestroyUIController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

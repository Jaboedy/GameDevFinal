using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneController : MonoBehaviour
{
    Player player;
    UIController uiController;
    AudioPlayer audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        uiController = FindObjectOfType<UIController>();
        if (player)
        {
            player.DestroyPlayer();
        }
        
        uiController.DestroyUIController();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        if (audioPlayer)
        {
            audioPlayer.DestroyAudioPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(3);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

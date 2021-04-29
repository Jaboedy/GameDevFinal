using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    [SerializeField] SpawnPoint[] spawnPoints;
    [SerializeField] ExitPoint[] exitPoints;

    Player player;

    int spawnID = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.GetSceneController();
        spawnID = player.GetSpawnID();
        var spawns = MapSpawns();
        SpawnPlayer(spawns);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Dictionary<int, SpawnPoint> MapSpawns()
    {
        Dictionary<int, SpawnPoint> spawns = new Dictionary<int, SpawnPoint>();
        foreach(SpawnPoint spawn in spawnPoints)
        {
            spawns.Add(spawn.GetSpawnID(), spawn);
        }
        return spawns;
    }

    private void SpawnPlayer(Dictionary<int, SpawnPoint> s)
    {
        player.transform.position = s[spawnID].transform.position;
    }

    public void SetSpawnID(int newID)
    {
        player.SetSpawnID(newID);
    }

    public void LoadNextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadDeathScene()
    {
        SceneManager.LoadScene(1);
    }
}

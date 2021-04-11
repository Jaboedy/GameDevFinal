using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    [SerializeField] SpawnPoint[] spawnPoints;
    [SerializeField] ExitPoint[] exitPoints;
    [SerializeField] Player player;

    int spawnID = 0;

    // Start is called before the first frame update
    void Start()
    {
        var spawns = MapSpawns();
        SpawnPlayer(spawns);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetExitID(int id)
    {
        spawnID = id;
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
        spawnID = newID;
    }
}

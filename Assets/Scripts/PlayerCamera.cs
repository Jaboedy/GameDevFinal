using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{

    Player player;
    CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

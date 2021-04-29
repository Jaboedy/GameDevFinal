using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] swordSounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwingAudio()
    {
        if (swordSounds.Length > 1)
        {
            AudioSource.PlayClipAtPoint(swordSounds[0], Camera.main.transform.position, 0.15f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            AudioSource.PlayClipAtPoint(swordSounds[1], Camera.main.transform.position, 0.15f);
        }
    }
}

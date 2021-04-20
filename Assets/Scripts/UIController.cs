using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject[] health;
    [SerializeField] GameObject[] mana;
    // Start is called before the first frame update

    Player player;
    int currentHP;
    int currentMana;

    private void Awake()
    {
        int controllerCount = FindObjectsOfType<UIController>().Length;
        if (controllerCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        currentHP = player.GetHP();
        currentMana = player.GetMana();

        SetUIHealth();
        SetUIMana();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUIHealth()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < (currentHP))
            {
                health[i].SetActive(true);
            }
            else
            {
                health[i].SetActive(false);
            }
                
        }
    }

    public void SetUIMana()
    {
        for (int i = 0; i < 3; i++)
        {
            if(i < (currentMana))
            {
                mana[i].SetActive(true);
            }
            else
            {
                mana[i].SetActive(false);
            }
        }
    }

    public void SetHP(int playerHP)
    {
        currentHP = playerHP;
        SetUIHealth();
    }
}

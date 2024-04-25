using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public int team;
    public GameObject[] cageSpawnPoints;
    public bool isCaged = false;

    private GameObject gameManagerObject;
    private Animator animator;
    private float defaultSpeed;
    private AIDestinationSetter aIDestinationSetter;
    private BotLogic botLogic;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        cageSpawnPoints = GameObject.FindGameObjectsWithTag("CageSpawnPoint");
        defaultSpeed = gameObject.GetComponent<BirdScript>().maxSpeed;

        aIDestinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        botLogic = gameObject.GetComponent<BotLogic>();
    }

    //  Team 0 - slimes, Team 1 - goblins
    void Update()
    {
        if(GameManager.isCreaturesWin)
        {
            if (gameObject.tag == "PlayerTeam0")
                animator.SetBool("isWinner", true);
        }
        else if (GameManager.isGoblinsWin)
        {
            if (gameObject.tag == "PlayerTeam1")
                animator.SetBool("isWinner", true);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "PlayerTeam1" && col.gameObject.GetComponent<Player>().team == 1
            && gameObject.GetComponent<Player>().team == 0)
        {
            Debug.Log("Slime captured goblin");
            int random = Random.Range(0, cageSpawnPoints.Length);
            col.gameObject.transform.position = cageSpawnPoints[random].transform.position;
            col.gameObject.GetComponent<Player>().isCaged = true;
            gameManager.UpdateCount();
            gameObject.GetComponent<PlayerStatistic>().GoblinsCaptured++;

            var botLogicScript = col.gameObject.GetComponent<BotLogic>();
            if (botLogicScript != null)
            {
                col.gameObject.GetComponent<BotLogic>().FindNextTarget();

            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag =="CageTrigger" && gameObject.GetComponent<Player>().team == 1)
        {
            GameManager.goblins--;
            gameManager.UpdateCount();
        }
        if(col.gameObject.tag == "SwapTrigger")
        {
            gameObject.GetComponent<BirdScript>().maxSpeed = defaultSpeed / 2;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "CageTrigger" && gameObject.GetComponent<Player>().team == 1)
        {
            GameManager.goblins++;
            gameManager.UpdateCount();
            isCaged = false;
        }
        if (col.gameObject.tag == "SwapTrigger")
        {
            gameObject.GetComponent<BirdScript>().maxSpeed = defaultSpeed;
        }
    }
}

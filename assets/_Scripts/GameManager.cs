using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static int moneyBags = 0;
    public static int goblins = 0;
    public int maxGoblins;
    public static int creatures = 0;
    public Text bagsCount;
    public Text goblinsCount;
    public Text startGoblinsCount;
    public Text startCreaturesCount;
    public Text winText;
    public Text goblinsMvp;
    public Text creaturesMvp;
    public static bool isGameStarted = false;
    public static bool isGoblinsWin;
    public static  bool isCreaturesWin;
    public GameObject startGameCanvas;
    public GameObject mainGameCanvas;
    public GameObject winCanvas;
    public Vector2 winnerCoordiantes;

    private BotLogic botLogic;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        GetGoblinsBags();

        bagsCount.text = "Bags Left: " + moneyBags;
        goblinsCount.text = "Goblins Left: " + goblins;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted)
            Camera.main.GetComponent<AudioSource>().enabled = true;

        if(moneyBags <= 0 && isGameStarted)
        {
            winCanvas.SetActive(true);
            isGameStarted = false;
            winText.text = "GOBLINS WIN!";
            isGoblinsWin = true;
        }
        if(goblins <= 0 && isGameStarted)
        {
            winCanvas.SetActive(true);
            isGameStarted = false;
            winText.text = "CREATURES WIN!";
            isCreaturesWin = true;

        }

        if (isGoblinsWin || isCreaturesWin)
        {
            var allCreatureStatistics = GameObject.FindGameObjectsWithTag("PlayerTeam0")
                .ToList();
            var allGoblinStatistic = GameObject.FindGameObjectsWithTag("PlayerTeam1")
                .ToList();

            // finds the best pair
            var bestCreature = allCreatureStatistics.Aggregate((x, y) => x.GetComponent<PlayerStatistic>().GoblinsCaptured > y.GetComponent<PlayerStatistic>().GoblinsCaptured ? x : y);
            var bestGoblin = allGoblinStatistic.Aggregate((x, y) => x.GetComponent<PlayerStatistic>().BagsCollected > y.GetComponent<PlayerStatistic>().BagsCollected ? x : y);

            var bestCreatureStatistic = bestCreature.GetComponent<PlayerStatistic>();
            var bestGoblinStatistic = bestGoblin.GetComponent<PlayerStatistic>();

            creaturesMvp.color = bestCreatureStatistic.PlayerColor;
            goblinsMvp.color = bestGoblinStatistic.PlayerColor;
            creaturesMvp.text = $"{bestCreature.GetComponent<HFTGamepad>().playerName} captured {bestCreatureStatistic.GoblinsCaptured} {(bestCreatureStatistic.GoblinsCaptured > 1 ? "goblins" : "goblin")}!";
            goblinsMvp.text = $"{bestGoblin.GetComponent<HFTGamepad>().playerName} collected {bestGoblinStatistic.BagsCollected} {(bestGoblinStatistic.BagsCollected > 1 ? "bags" : "bag")}!";

            Camera.main.transform.position = isGoblinsWin
                ? Vector3.Lerp(Camera.main.transform.position, new Vector3(bestGoblin.transform.position.x, bestGoblin.transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 2)
                : Vector3.Lerp(Camera.main.transform.position, new Vector3(bestCreature.transform.position.x, bestCreature.transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 2);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, Time.deltaTime * 2);
        }


        if (!isGameStarted)
            maxGoblins = GameObject.FindGameObjectsWithTag("PlayerTeam1").Length;
        if (isGameStarted)
        {
            int newMaxGoblins = GameObject.FindGameObjectsWithTag("PlayerTeam1").Length;
            if (maxGoblins != newMaxGoblins)
            {
                goblins = goblins - (maxGoblins - newMaxGoblins);
                maxGoblins = newMaxGoblins;
                UpdateCount();
            }
        }
    }

    public void UpdateCount()
    {
        Debug.Log("Goblins left:" + goblins);
        bagsCount.text = "Chests Left: " + moneyBags;
        goblinsCount.text = "Goblins Left: " + goblins;
    }

    public void GetGoblinsBags()
    {
        moneyBags = GameObject.FindGameObjectsWithTag("Bag").Length;
        goblins = GameObject.FindGameObjectsWithTag("PlayerTeam1").Length;
        creatures = GameObject.FindGameObjectsWithTag("PlayerTeam0").Length;
        startGoblinsCount.text = "Goblins Connected: " + goblins;
        startCreaturesCount.text = "Creatures Connected: " + creatures;

    }

    public void StartGame()
    {
        Debug.Log("Game started!");
        isGameStarted = true;
        startGameCanvas.GetComponent<Canvas>().enabled = false;
        mainGameCanvas.GetComponent<Canvas>().enabled = true;
        botLogic.FindNextTarget();
        botLogic.findnNextBag();
    }


    public void DecreaseGoblins()
    {
        goblins--;
    }
}

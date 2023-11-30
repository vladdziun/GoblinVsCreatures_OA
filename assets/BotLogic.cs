using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotLogic : MonoBehaviour
{
    private AIPath aiPath;
    private AIDestinationSetter aIDestinationSetter;
    private List<GameObject> goblinPlayerList = new List<GameObject>();
    void Start()
    {
        aiPath = gameObject.GetComponent<AIPath>();
        aIDestinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        goblinPlayerList = GameObject.FindGameObjectsWithTag("PlayerTeam1").ToList();
        Debug.Log(goblinPlayerList.Count);


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameStarted && aiPath.canMove == false)
        {
            aiPath.canMove = true;
            FindNextTarget();
        }
    }

    public void FindNextTarget()
    {
        if (GameManager.isGameStarted)
        {
            Debug.Log("Looking for new target");
            goblinPlayerList = GameObject.FindGameObjectsWithTag("PlayerTeam1").ToList();
            Debug.Log("before: " + goblinPlayerList.Count);
            var filteredList = new List<GameObject>();
            foreach (var goblin in goblinPlayerList)
            {
                if (!goblin.GetComponent<Player>().isCaged)
                {
                    filteredList.Add(goblin);
                }
            }
            Debug.Log("after: " + goblinPlayerList.Count);


            aIDestinationSetter.target = filteredList.FirstOrDefault().transform;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSpawner : MonoBehaviour
{
    public GameObject[] collectibles;
    public List<GameObject> spawnPointsList;

    private Queue<GameObject> spawnPoints;


    void Start()
    {
        spawnPoints = new Queue<GameObject>(spawnPointsList);
        StartCoroutine(SpawnCollectible());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnCollectible()
    {
        if(GameManager.isGameStarted)
        {
            int random = Random.Range(0, collectibles.Length);
            GameObject newCollectible = Instantiate(collectibles[random], new Vector3(spawnPoints.Peek().transform.position.x,
                        spawnPoints.Peek().transform.position.y, 0), Quaternion.identity);
            spawnPoints.Enqueue(spawnPoints.Dequeue());
        }

        yield return new WaitForSeconds(5);
        StartCoroutine(SpawnCollectible());
    }
}

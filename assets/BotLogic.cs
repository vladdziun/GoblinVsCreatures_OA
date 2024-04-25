using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class BotLogic : MonoBehaviour
{

    public GameObject potentialTarget;
    private AIPath aiPath;
    private AIDestinationSetter aIDestinationSetter;
    private List<GameObject> goblinPlayerList = new List<GameObject>();
    List<GameObject> previouslySelectedTargets = new List<GameObject>();

    string[] traditionalUkrainianMaleNames = new string[]
    {
        "Богдан",
        "Василь",
        "Григорій",
        "Данило",
        "Євгеній",
        "Ждан",
        "Зеновій",
        "Ілля",
        "Кирило",
        "Леонід",
        "Микита",
        "Нестор",
        "Остап",
        "Петро",
        "Ростислав",
        "Семен",
        "Тимофій",
        "Федір",
        "Хведір",
        "Ярослав"
    };

    void Awake()
    {
        int randomIndex = Random.Range(0, traditionalUkrainianMaleNames.Length);
        string randomName = traditionalUkrainianMaleNames[randomIndex];

        gameObject.GetComponent<HFTGamepad>().playerName = randomName;
        gameObject.GetComponent<HFTGamepad>().Name = randomName;
    }

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
            if (gameObject.tag == "PlayerTeam0")
            {
                FindNextTarget();
            }
            else if (gameObject.tag == "PlayerTeam1")
            {
                findnNextBag();
            }
        }

        if (potentialTarget != null && potentialTarget.GetComponent<Player>().isCaged && GameManager.isGameStarted)
        {
            FindNextTarget();
        }
    }

    public void FindNextTarget()
    {
        if (GameManager.isGameStarted)
        {
            Debug.Log("Looking for new target");
            var goblinPlayerList = GameObject.FindGameObjectsWithTag("PlayerTeam1").ToList();
            var filteredList = goblinPlayerList.Where(goblin => !goblin.GetComponent<Player>().isCaged).ToList();

            TryFindNewTarget(filteredList, 0);
        }
    }

    private void TryFindNewTarget(List<GameObject> potentialTargets, int index)
    {
            GameObject potentialTarget = potentialTargets[index];

            // Check if the potential target is caged, if so, skip to the next target
            if (potentialTarget.GetComponent<Player>().isCaged || potentialTarget == aIDestinationSetter.target)
            {
                TryFindNewTarget(potentialTargets, index + 1);
                return;
            }

            Vector3 targetPos2D = potentialTarget.transform.position;
            CheckPath(targetPos2D, (path) =>
            {
                Vector3 pathEndPos2D = new Vector3(path.vectorPath.Last().x, path.vectorPath.Last().y, targetPos2D.z);
                if (path.CompleteState == PathCompleteState.Complete && Vector2.Distance(pathEndPos2D, targetPos2D) < 5f)
                {
                    // Path is complete and ends at the target position
                    aIDestinationSetter.target = potentialTarget.transform;
                }
                else
                {
                    // Try the next target
                    TryFindNewTarget(potentialTargets, index + 1);
                }
            });
        
    }

    private void CheckPath(Vector3 targetPosition, System.Action<ABPath> onComplete)
    {
        var path = ABPath.Construct(transform.position, targetPosition, (p) =>
        {
            onComplete?.Invoke(p as ABPath);
        });

        AstarPath.StartPath(path);
    }
    public void findnNextBag()
    {
        //if (GameManager.isGameStarted && !gameObject.GetComponent<Player>().isCaged && gameObject.tag == "PlayerTeam1")
        //{
        //    var bags = GameObject.FindGameObjectsWithTag("Bag").ToList();
        //    //foreach (var bag in bags.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)))
        //    //{
        //    //    //CheckPath(bag.transform.position, (path) =>
        //    //    //{
        //    //    //    if (path.CompleteState == PathCompleteState.Complete)
        //    //    //    {
        //    //            // Path is complete and target is reachable
        //    //            aIDestinationSetter.target = bag.transform;
        //    //            return;
        //    //    //    }
        //    //    //});
        //    //}
        //aIDestinationSetter.target = bags.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault().transform;
        //}

        StartCoroutine(FindBag());
    }

    IEnumerator FindBag()
    {
        // Wait until end of frame so that Unity has a chance to actually remove the destroyed object
        yield return new WaitForEndOfFrame();
        if (GameManager.isGameStarted && !gameObject.GetComponent<Player>().isCaged && gameObject.tag == "PlayerTeam1")
        {
            var bags = GameObject.FindGameObjectsWithTag("Bag").ToList();
            //foreach (var bag in bags.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)))
            //{
            //    //CheckPath(bag.transform.position, (path) =>
            //    //{
            //    //    if (path.CompleteState == PathCompleteState.Complete)
            //    //    {
            //            // Path is complete and target is reachable
            //            aIDestinationSetter.target = bag.transform;
            //            return;
            //    //    }
            //    //});
            //}
            aIDestinationSetter.target = bags.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault().transform;
        }

    }
}


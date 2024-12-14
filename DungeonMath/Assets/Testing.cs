using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public bool pathDebug;
    public bool gridDebug;

    private enum State
    {
        PATROL,
        CHASE,
        SEARCH
    }
    private State state;
    public GameObject player;
    private Vector3 lastKnown = Vector3.negativeInfinity;

    private Pathfinding pathfinding;
    EnemyMovement movement;

    private int patrolIndex = 0;
    Vector3 patrolPosition;
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    public GameObject waypoint4;
    private GameObject[] patrol;

    private List<Vector3> search;
    private Vector3 currentSearch;
    public float searchRadius;
    public float searchTime;
    private float searchClock;

    public float detectFOV;
    public float detectRange;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        state = State.PATROL;
    }

    private void Start()
    {
        pathfinding = new Pathfinding(70, 46, new Vector3(-0.8f, 0, -0.8f));

        //Main walls
            //hall/kitchen dividing wall
            pathfinding.placeWallVert(21, 0, 10);//lower
            pathfinding.placeWallVert(21, 12, 22);//upper
            //dining/treasure dividing wall
            pathfinding.placeWallVert(21, 47, 58);//lower
            pathfinding.placeWallVert(21, 60, 69);//upper
            //center square
            pathfinding.placeWallVert(6, 23, 47);//right
            pathfinding.placeWallVert(31, 23, 34);//left lower
            pathfinding.placeWallVert(31, 36, 47);//left upper
            pathfinding.placeWallHoriz(47, 6, 31);//top
            pathfinding.placeWallHoriz(23, 6, 32);//bottom
        //hall
            //benches
            pathfinding.placeWallVert(5, 5, 9); //back row
            pathfinding.placeWallVert(5, 14, 18);
            pathfinding.placeWallVert(7, 5, 9); //second row
            pathfinding.placeWallVert(7, 14, 18);
            pathfinding.placeWallVert(9, 5, 9); //third row
            pathfinding.placeWallVert(9, 14, 18);
            pathfinding.placeWallVert(11, 5, 9); //fourth row
            pathfinding.placeWallVert(11, 14, 18);
            pathfinding.placeWallVert(13, 5, 9); //fifth row
            pathfinding.placeWallVert(13, 14, 18);
            pathfinding.placeWallVert(15, 5, 9); //front row
            pathfinding.placeWallVert(15, 14, 18);
            //table
            pathfinding.placeWallVert(17, 10, 13);
            pathfinding.placeWallVert(18, 10, 13);
        //kitchen
            //table
            pathfinding.placeWallVert(26, 7, 14);
            pathfinding.placeWallVert(27, 7, 14);
            pathfinding.placeWallVert(28, 7, 14);
            pathfinding.placeWallVert(29, 7, 14);
            //cauldrons
            pathfinding.placeWallHoriz(7, 33, 41);
            //ovens
            pathfinding.placeWallHoriz(0, 25, 31);
            pathfinding.placeWallHoriz(1, 25, 31);
            pathfinding.placeWallHoriz(0, 35, 41);
            pathfinding.placeWallHoriz(1, 35, 41);
        //barrels
        //Big barrel by kitchen
        pathfinding.placeWallVert(40, 13, 21);
            pathfinding.placeWallVert(41, 13, 21);
            //Side Wall Barrels
            pathfinding.placeWallVert(44, 25, 29);
            pathfinding.placeWallVert(45, 25, 29);
            //Chest
            pathfinding.placeWallVert(44, 32, 34);
            pathfinding.placeWallVert(45, 32, 34);
            //Side wall Barrels
            pathfinding.placeWallVert(44, 38, 42);
            pathfinding.placeWallVert(45, 38, 42);
            //Big barrel by dining
            pathfinding.placeWallHoriz(45, 39, 45);
            pathfinding.placeWallHoriz(46, 39, 45);
            //barrels in front of library
            pathfinding.placeWallVert(39, 31, 34);
            pathfinding.placeWallVert(34, 26, 30);
            pathfinding.placeWallVert(34, 36, 39);
        //dining
        //table
        pathfinding.placeWallHoriz(51, 26, 37);
            pathfinding.placeWallHoriz(52, 26, 37);
            pathfinding.placeWallHoriz(53, 26, 37);
            pathfinding.placeWallHoriz(54, 26, 37);
            pathfinding.placeWallHoriz(55, 26, 37);
            //other table
            pathfinding.placeWallHoriz(61, 26, 37);
            pathfinding.placeWallHoriz(62, 26, 37);
            pathfinding.placeWallHoriz(63, 26, 37);
            pathfinding.placeWallHoriz(64, 26, 37);
            //weapon rack
            pathfinding.placeWallVert(43, 51, 63);
        //treasure room
            //chest
            pathfinding.placeWallVert(9, 58, 60);
            pathfinding.placeWallVert(10, 58, 60);
        //library
            pathfinding.placeWallVert(9, 29, 33);
            pathfinding.placeWallVert(9, 37, 41);
            pathfinding.placeWallVert(12, 29, 33);
            pathfinding.placeWallVert(12, 37, 41);
            pathfinding.placeWallVert(15, 29, 33);
            pathfinding.placeWallVert(15, 37, 41);
            pathfinding.placeWallVert(18, 29, 33);
            pathfinding.placeWallVert(18, 37, 41);
            pathfinding.placeWallVert(21, 29, 33);
            pathfinding.placeWallVert(21, 37, 41);
            pathfinding.placeWallVert(24, 29, 33);
            pathfinding.placeWallVert(24, 37, 41);

        if (gridDebug)  pathfinding.drawGridDebug(); 

        patrol = new GameObject[] { waypoint1, waypoint2, waypoint3, waypoint4 };
        patrolPosition = getNextPatrolPosition();
    }

    private void Update()
    {
        switch (state)
        {
            case State.PATROL:
                moveTo(patrolPosition);
                if (destinationReached(patrolPosition))
                {
                    Debug.Log("Reached patrol position, pausing");
                    movement.StopMoving();
                    patrolPosition = getNextPatrolPosition();
                    //get2RandomPositons();
                    //state = State.SEARCH;
                }
                searchForPlayer();
                break;

            case State.CHASE:
                Debug.Log("Chasing...");
                moveTo(lastKnown);
                if (destinationReached(player.transform.position)) 
                {
                    Debug.Log("Caught player");
                    movement.StopMoving();
                    state = State.PATROL;
                    break;
                } else if (destinationReached(lastKnown))
                {
                    Debug.Log("Lost player, beginning search");
                    movement.StopMoving();
                    //get2RandomPositons();
                    //state = State.SEARCH;
                    state = State.PATROL;
                }
                searchForPlayer();
                break;

            case State.SEARCH:
                moveTo(currentSearch);
                if (destinationReached(currentSearch))
                {
                    Debug.Log("Searched position, moving on...");
                    if (search.Count <= 0)
                    {
                        Debug.Log("Out of positions to search");
                        state = State.PATROL;
                    }
                    getNextSearchPosition();
                }
                break;
        }

    
    }

    private void get2RandomPositons()
    {
        Vector3 origin = transform.position;
        Vector3 pos1 = new Vector3(origin.x + Random.Range(-searchRadius, searchRadius), 0, origin.z + Random.Range(-searchRadius, searchRadius));
        Vector3 pos2 = new Vector3(origin.x + Random.Range(-searchRadius, searchRadius), 0, origin.z + Random.Range(-searchRadius, searchRadius));

        List<Vector3> testPath = pathfinding.FindPath(transform.position, pos1);
        while(testPath == null)
        {
            pos1 = new Vector3(origin.x + Random.Range(-searchRadius, searchRadius), 0, origin.z + Random.Range(-searchRadius, searchRadius));
            testPath = pathfinding.FindPath(transform.position, pos1);
        }
        testPath = pathfinding.FindPath(transform.position, pos2);
        while(testPath == null)
        {
            pos2 = new Vector3(origin.x + Random.Range(-searchRadius, searchRadius), 0, origin.z + Random.Range(-searchRadius, searchRadius));
            testPath = pathfinding.FindPath(transform.position, pos2);
        }

        search = new List<Vector3> { pos1, pos2 };
        getNextSearchPosition();
    }



    private void getNextSearchPosition()
    {
        if(search.Count <= 0)
        {
            state = State.PATROL;
            return;
        }
        currentSearch = search[0];
        search.RemoveAt(0);
    }

    private Vector3 getNextPatrolPosition()
    {
        if(patrolIndex >= patrol.Length)
        {
            patrolIndex = 0;
        }
        Vector3 pos = patrol[patrolIndex].transform.position;
        patrolIndex++;
        return pos;
    }

    private Vector3 getPosFromMouse()
    {
        Vector3 mouse = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouse);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            return point;
        }
        else
        {
            return (Vector3.zero);
        }
    }

    private void moveTo(Vector3 dest)
    {
        List<Vector3> path = pathfinding.FindPath(transform.position, dest);
        if(path != null)
        {
            movement.ContinuePath(path);
            if(pathDebug) pathfinding.drawPathDebug(path);
        }
    }

    private void chase()
    {
        List<Vector3> path = pathfinding.FindPath(transform.position, player.transform.position);
        if(path != null)
        {
            movement.NewPath(path);
            pathfinding.drawPathDebug(path);
        }
    }

    private bool destinationReached(Vector3 goal) 
    {
       if (Vector3.Distance(transform.position, goal) < 0.5f)
        {
            Debug.Log("Destination Reached");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void searchForPlayer()
    {
        if (pathDebug)
        {
            DrawDebugFOV();
        }
        if (canSeePlayer())
        {
            Debug.Log("Player Spotted");
            lastKnown = player.transform.position;
            state = State.CHASE;
        }
    }
       
         

    private bool canSeePlayer()
    {
        Vector3 playerBody = player.transform.position + Vector3.up * 1f;
        Vector3 npcBody = transform.position + Vector3.up * 1.5f; ;
        Vector3 dirToPlayer = playerBody - npcBody;
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (dirToPlayer.magnitude > detectRange || angleToPlayer > detectFOV/2) return false; //player is out of view and/or range

        if (checkLinesOfSight(dirToPlayer)) return true;
        Debug.Log("Raycast fail, cannot see player"); 
        return false;
    }

    private bool checkLinesOfSight(Vector3 playerDir)
    {
        float spread = 5f;
        Vector3 left = Quaternion.AngleAxis(-spread, Vector3.up) * playerDir;
        Vector3 right = Quaternion.AngleAxis(spread, Vector3.up) * playerDir;
        Vector3 npcBody = transform.position + Vector3.up * 1.5f; ;

        if (pathDebug)
        {
            Debug.DrawRay(npcBody, playerDir * detectRange, Color.cyan, 0f);
            Debug.DrawRay(npcBody, left * detectRange, Color.cyan, 0f);
            Debug.DrawRay(npcBody, right * detectRange, Color.cyan, 0f);
        }


        Ray lineOfSight = new Ray(npcBody, playerDir);
        Ray leftRay = new Ray(npcBody, left);
        Ray rightRay = new Ray(npcBody, right);


        Ray[] rays = new Ray[] { lineOfSight, leftRay, rightRay };
        bool playerHit = false;
        Collider playerCollider = player.GetComponent<Collider>();


        foreach(Ray r in rays)
        {
            if(Physics.Raycast(r, out RaycastHit hit, detectRange) && hit.transform == player.transform)
            {
                playerHit = true;
            }
        }
        return playerHit;
    }

    private void DrawDebugFOV()
    {
        Quaternion leftRotation = Quaternion.Euler(0, -detectFOV / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, detectFOV / 2, 0);

        Vector3 leftBoundary = leftRotation * transform.forward * detectRange;
        Vector3 rightBoundary = rightRotation * transform.forward * detectRange;

        Debug.DrawLine(transform.position, transform.position + leftBoundary, Color.yellow,0f);
        Debug.DrawLine(transform.position, transform.position + rightBoundary, Color.yellow,0f);

    }

}

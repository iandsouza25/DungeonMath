using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public bool debug;
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
        pathfinding = new Pathfinding(100, 100, new Vector3(-0.8f, 0, -0.8f));

        patrol = new GameObject[] { waypoint1, waypoint2 };
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
            pathfinding.drawPathDebug(path);
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
        if (debug)
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

        if (debug)
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

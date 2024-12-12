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
        CHASE
    }
    private State state;
    public GameObject player;

    private Pathfinding pathfinding;
    EnemyMovement movement;

    private int patrolIndex = 0;
    Vector3 patrolPosition;
    public GameObject waypoint1;
    public GameObject waypoint2;

    private GameObject[] patrol;

    public float detectFOV;
    public float detectRange;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        state = State.PATROL;
    }

    private void Start()
    {
        pathfinding = new Pathfinding(100, 100);

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
                    moveTo(patrolPosition);
                }
                searchForPlayer();
                break;
            case State.CHASE:
                Debug.Log("Chasing...");
                moveTo(player.transform.position);
                if (destinationReached(player.transform.position))
                {
                    Debug.Log("Caught player");
                    movement.StopMoving();
                    state = State.PATROL;
                }
                searchForPlayer();
                break;
        }

    
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
       if (Vector3.Distance(transform.position, goal) < movement.destThreshold)
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
            state = State.CHASE;
        } else
        {
            state = State.PATROL;
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

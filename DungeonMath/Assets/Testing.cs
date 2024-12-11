using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Testing : MonoBehaviour
{
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
        pathfinding = new Pathfinding(15, 9);

        pathfinding.placeWallVert(6, 0, 6);
        pathfinding.placeWallVert(5, 0, 6);

        pathfinding.placeWallVert(6, 8, 12);
        pathfinding.placeWallVert(5, 8, 12);

        pathfinding.placeWallVert(4, 11, 12);

        pathfinding.placeWallVert(3,2, 12);
        pathfinding.placeWallVert(2,2, 12);

        pathfinding.placeWallHoriz(8, 0, 1);
        pathfinding.placeWallHoriz(9, 0, 1);

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
                    movement.StopMoving();
                    patrolPosition = getNextPatrolPosition();
                    moveTo(patrolPosition);
                }
                //searchForPlayer();
                break;
            case State.CHASE:
                moveTo(player.transform.position);
                if (destinationReached(player.transform.position))
                {
                    movement.StopMoving();
                    state = State.PATROL;
                }
                //searchForPlayer();
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.transform.position = getPosFromMouse();
            state = State.CHASE;
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
        return (Vector3.Distance(transform.position, goal) < 0.5f);
    }

    private void searchForPlayer()
    {
        DrawDebugFOV();
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
        Vector3 dirToPlayer = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (dirToPlayer.magnitude > detectRange || angleToPlayer > detectFOV/2) return false; //player is out of view and/or range

        Ray lineOfSight = new Ray(transform.position, dirToPlayer);
        if (Physics.Raycast(lineOfSight, out RaycastHit hit, detectRange) && hit.transform == player.transform) return true;

        return false;
    }

    private void DrawDebugFOV()
    {
        Quaternion leftRotation = Quaternion.Euler(0, -detectFOV / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, detectFOV / 2, 0);

        Vector3 leftBoundary = leftRotation * transform.forward * detectRange;
        Vector3 rightBoundary = rightRotation * transform.forward * detectRange;

        Debug.DrawLine(transform.position, transform.position + leftBoundary, Color.yellow,1f);
        Debug.DrawLine(transform.position, transform.position + rightBoundary, Color.yellow,1f);

    }

}

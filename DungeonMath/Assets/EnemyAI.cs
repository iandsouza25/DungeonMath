using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public TMP_Text caughtMessageText;
    public CharacterController controller;
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
  
    private GameObject[] patrol;
    public float detectFOV;
    public float detectRange;

    AudioSource audioSrc;
    private float lastSoundTime;
    private float soundCoolDown = 1f;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        state = State.PATROL;
        audioSrc = GetComponent<AudioSource>();
        pathfinding = new Pathfinding(70, 46, new Vector3(-0.8f, 0, -0.8f));

    }

    private void Start()
    {
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

        patrol = new GameObject[] { waypoint1, waypoint2, waypoint3};
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
                }
                searchForPlayer();
                break;

            case State.CHASE:
                if(Time.time >= lastSoundTime + soundCoolDown) 
                {
                    audioSrc.Play();
                    lastSoundTime = Time.time;
                }
                moveTo(lastKnown);
                if (destinationReached(player.transform.position)) 
                {
                    StartCoroutine(DisplayCaughtMessage());
                    break;
                } else if (destinationReached(lastKnown))
                {
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
        Collider playerCollider = player.GetComponent<Collider>();

        Vector3 playerBody = player.transform.position; playerBody.y = 0.8f;
        Vector3 npcBody = transform.position; npcBody.y = 0.8f;
        Vector3 playerLegs = player.transform.position; playerLegs.y = 0.2f; ;
        Vector3 npcLegs = transform.position; npcLegs.y = 0.2f;
        Vector3 dirToPlayer = playerBody - npcBody;
        Vector3 dirToLegs = playerLegs - npcLegs;
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (dirToPlayer.magnitude > detectRange || angleToPlayer > detectFOV/2) return false; //player is out of view and/or range

        if (checkLinesOfSight(dirToPlayer,dirToLegs)) return true;
        return false;
    }

    private bool checkLinesOfSight(Vector3 playerBody, Vector3 playerLegs)
    {
        Vector3 npcBody = transform.position + Vector3.up * 1.5f;
        Vector3 npcLegs = transform.position + Vector3.up * 0.5f;
        Ray lineOfSight = new Ray(npcBody, playerBody);
        Ray lowerLineOfSight = new Ray(npcLegs, playerLegs);

        if (pathDebug)
        {
            Debug.DrawRay(npcBody, playerBody * detectRange, Color.cyan, 0f);
            Debug.DrawRay(npcLegs, playerLegs * detectRange, Color.cyan, 0f);
        }

        Ray[] rays = new Ray[] { lineOfSight, lowerLineOfSight };
        Collider playerCollider = player.GetComponent<Collider>();

        bool playerHit = false;
        foreach (Ray r in rays)
        {
            Physics.Raycast(r, out RaycastHit hit, detectRange);
            if ( hit.transform == player.transform)
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

    private IEnumerator DisplayCaughtMessage()
    {
        caughtMessageText.gameObject.SetActive(true); // Show the message
        caughtMessageText.text = "YOU'VE BEEN CAUGHT! RESETTING THE LEVEL...";
        movement.StopMoving();
        controller.enabled = false;
        
        yield return new WaitForSeconds(3);
        caughtMessageText.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

}

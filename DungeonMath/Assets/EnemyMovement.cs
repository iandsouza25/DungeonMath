using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private List<Vector3> path;
    private int currIdx = 0;
    public float speed;
    public float destThreshold;
    void Start()
    {
        path = null;
    }

    void FixedUpdate()
    {
        if(path == null || path.Count == 0) return;
        if (currIdx >= path.Count)
        {
            currIdx = 0;
            path = null;
            return;
        }
        Vector3 target = path[currIdx];
        transform.position = Vector3.MoveTowards(transform.position, target, speed);

        if (Vector3.Distance(transform.position, target) < destThreshold)
        {
            currIdx++;
        }
    }

    public void ContinuePath(List<Vector3> newPath)
    {
        path = newPath;
    }

    public void NewPath(List<Vector3> newPath)
    {
        path = newPath;
        currIdx = 0;
    }

    public void StopMoving()
    {
        path = null;
        currIdx = 0;
    }
    
}

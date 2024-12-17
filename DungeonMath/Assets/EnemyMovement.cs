using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private List<Vector3> path;
    private int currIdx = 0;
    private float destThreshold = 0.5f;

    private float walkSpeed = 0.05f;
    private float runSpeed;
    private float currentSpeed = 0.05f;
    public float rotationSpeed;

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
        rotate(target); 
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed);
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

    public void startRunning()
    {
        currentSpeed = runSpeed;
    }

    public void setRunSpeed(float newSpeed)
    {
        runSpeed = newSpeed;
    }

    public void startWalking()
    {
        currentSpeed = walkSpeed;
    }

    public void StopMoving()
    {
        path = null;
        currIdx = 0;
    }

    public void rotate(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }
    
}

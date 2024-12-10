using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    Idle,
    WalkingForward,
    WalkingBackward,
    WalkingLeft,
    WalkingRight
}

public class CowboyController : MonoBehaviour
{
    private Animator animationController;
    private CharacterController characterController;
    private MovementState currentMovementState = MovementState.Idle;

    private bool isCrouching;
    private bool isRunning;
    private bool isBackstep;
    private bool isJumping;

    private float crouchVelocity = 1.5f;
    private float walkingVelocity = 2.0f;
    private float runningVelocity = 6.0f;
    private Vector3 movementDirection;
    private float velocity;


    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isCrouching = false;
        movementDirection = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardMovement;
        Vector3 lateralMovement;

        float mouseX = Input.GetAxis("Mouse X");
        float rotationSpeed = 2.0f;
        transform.Rotate(new Vector3(0.0f, mouseX * rotationSpeed, 0.0f));

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        bool isGrounded = characterController.isGrounded;
        if (Input.GetKey(KeyCode.B) && isGrounded)
        {
            animationController.SetBool("isBackstepJump", true);
            isCrouching = false;
        }
        else if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            animationController.SetBool("isJumpingForwards", true);
            isCrouching = false;
        }
        else
        {
            animationController.SetBool("isBackstepJump", false);
            animationController.SetBool("isJumpingForwards", false);
        }
        

        HandleMovementInput(ref isCrouching, ref isRunning);

        UpdateAnimator(ref isCrouching, ref isRunning);        

        bool isWalkingForwards = currentMovementState == MovementState.WalkingForward;
        bool isWalkingBackwards = currentMovementState == MovementState.WalkingBackward;
        bool isWalkingLeft = currentMovementState == MovementState.WalkingLeft;
        bool isWalkingRight = currentMovementState == MovementState.WalkingRight;


        if (isRunning)
        {
            velocity = Mathf.Clamp(velocity + 0.015f, 0, runningVelocity);
        }
        else if (isWalkingForwards && !isCrouching)
        {
            velocity = Mathf.Clamp(velocity + 0.01f, 0, walkingVelocity);
        }
        else if (isWalkingBackwards && !isCrouching)
        {
            velocity = Mathf.Clamp(velocity - 0.01f, walkingVelocity * -1, 0);
        }
        else if (isWalkingLeft && !isCrouching)
        {
            velocity = Mathf.Clamp(velocity - 0.01f, walkingVelocity * -1, 0);
        }
        else if (isWalkingRight && !isCrouching)
        {
            velocity = Mathf.Clamp(velocity + 0.01f, 0, walkingVelocity);
        }
        else if (isWalkingForwards && isCrouching)
        {
            velocity = Mathf.Clamp(velocity + 0.01f, 0, crouchVelocity);
        }
        else if (isWalkingBackwards && isCrouching)
        {
            velocity = Mathf.Clamp(velocity - 0.01f, crouchVelocity * -1, 0);
        }
        else if (isWalkingLeft && isCrouching)
        {
            velocity = Mathf.Clamp(velocity - 0.01f, crouchVelocity * -1, 0);
        }
        else if (isWalkingRight && isCrouching)
        {
            velocity = Mathf.Clamp(velocity + 0.01f, 0, crouchVelocity);
        }
        else
        {
            velocity = 0;
        }
        Debug.Log(velocity);


        if (isCrouching)
        {
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.35f, GetComponent<CapsuleCollider>().center.z);
        }
        else
        {
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.7f, GetComponent<CapsuleCollider>().center.z);
        }

        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        forwardMovement = new Vector3(xdirection, 0.0f, zdirection) * (isWalkingForwards || isWalkingBackwards ? 1 : 0);
        lateralMovement = new Vector3(-zdirection, 0.0f, xdirection) * (isWalkingLeft || isWalkingRight ? -1 : 0);

        movementDirection = (forwardMovement + lateralMovement).normalized;

        if (transform.position.y > 0.0f)
        {
            Vector3 lowerCharacter = movementDirection * velocity * Time.deltaTime;
            lowerCharacter.y = -100f;
            characterController.Move(lowerCharacter);
        }
        else
        {
            characterController.Move(movementDirection * velocity * Time.deltaTime);
        }
        
    }

    void HandleMovementInput(ref bool isCrouching, ref bool isRunning)
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentMovementState = MovementState.WalkingForward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentMovementState = MovementState.WalkingBackward;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            currentMovementState = MovementState.WalkingLeft;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentMovementState = MovementState.WalkingRight;
        }
        else
        {
            currentMovementState = MovementState.Idle;
        }

        if (Input.GetKey(KeyCode.LeftShift) && currentMovementState == MovementState.WalkingForward)
        {
            isRunning = true;
            isCrouching = false;
        }
        else
        {
            isRunning = false;
        }
    }

    void UpdateAnimator(ref bool isCrouching, ref bool isRunning)
    {
        animationController.SetBool("isWalkingForwards", currentMovementState == MovementState.WalkingForward);
        animationController.SetBool("isWalkingBackwards", currentMovementState == MovementState.WalkingBackward);
        animationController.SetBool("isWalkingLeft", currentMovementState == MovementState.WalkingLeft);
        animationController.SetBool("isWalkingRight", currentMovementState == MovementState.WalkingRight);
        animationController.SetBool("isRunning", isRunning);
        animationController.SetBool("isCrouch", isCrouching);
    }

    
}

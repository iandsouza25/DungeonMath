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
    private const float gravity = 7.8f;


    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animationController.SetBool("isWalkingForwards", true);
        animationController.SetBool("isWalkingForwards", false);

        isCrouching = false;
        movementDirection = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardMovement;
        Vector3 lateralMovement;

        float xdirection;
        float zdirection;

        float mouseX = Input.GetAxis("Mouse X");
        float rotationSpeed = 2.0f;
        transform.Rotate(new Vector3(0.0f, mouseX * rotationSpeed, 0.0f));

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        bool skipAnimation = false;
        bool isGrounded = characterController.isGrounded || animationController.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        if (Input.GetKey(KeyCode.B) && isGrounded)
        {
            skipAnimation = true;
            animationController.SetBool("isBackstepJump", true);
            isCrouching = false;
            Vector3 backwardDirection = -transform.forward;
            backwardDirection.y = 0.0f;
            backwardDirection = backwardDirection.normalized;

            float backstepDistance = 5.0f;
            Vector3 backstepMovement = backwardDirection * backstepDistance;

            characterController.Move(backstepMovement * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            skipAnimation = true;
            bool isSprinting = animationController.GetCurrentAnimatorStateInfo(0).IsName("Running");
            float multiplyFactor = !isSprinting ? 1.0f : 4.5f;

            animationController.SetBool("isJumpingForwards", true);
            isCrouching = false;
            Vector3 forwardDirection = transform.forward;

            forwardDirection.y = 0.0f;
            forwardDirection = forwardDirection.normalized;

            float forwardDistance = 5.0f;
            forwardMovement = forwardDirection * forwardDistance;

            characterController.Move(forwardMovement * Time.deltaTime * multiplyFactor);
        }
        else
        {
            animationController.SetBool("isBackstepJump", false);
            animationController.SetBool("isJumpingForwards", false);
        }
        
        isGrounded = characterController.isGrounded || animationController.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        skipAnimation = !isGrounded || skipAnimation;
        HandleMovementInput(ref isCrouching, ref isRunning, skipAnimation);

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


        if (isCrouching)
        {
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.35f, GetComponent<CapsuleCollider>().center.z);
        }
        else
        {
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.7f, GetComponent<CapsuleCollider>().center.z);
        }

        xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        forwardMovement = new Vector3(xdirection, 0.0f, zdirection) * (isWalkingForwards || isWalkingBackwards ? 1 : 0);
        lateralMovement = new Vector3(-zdirection, 0.0f, xdirection) * (isWalkingLeft || isWalkingRight ? -1 : 0);

        movementDirection = (forwardMovement + lateralMovement).normalized;
        movementDirection.y -= gravity * Time.deltaTime;

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

    void HandleMovementInput(ref bool isCrouching, ref bool isRunning, bool skipAnimation)
    {
        if (skipAnimation)
        {
            return;
        }

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

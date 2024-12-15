using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CowboyController : MonoBehaviour
{
    private Animator animationController;
    private CharacterController characterController;

    private bool isCrouching;
    private bool isRunning;
    private bool isBackstep;
    private bool isJumping;

    private float crouchVelocity = 3.0f;
    private float walking_velocity = 4.0f;
    private float runningVelocity = 8.0f;
    private Vector3 movementDirection;
    private float velocity;
    private const float gravity = 15f;

    public float health;
    public bool has_won;

    private bool isDead;

    private bool isWalkingBackwards;
    private bool isWalkingForwards;
    private bool isWalkingLeft;
    private bool isWalkingRight;


    private float verticalVelocity; // Add this variable to manage vertical movement
    private const float jumpForce = 7.0f; // Jump strength

    


    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        velocity = 0.0f;
        has_won = false;
        isDead = false;
        health = 100.0f;
        movementDirection = new Vector3(0.0f, 0.0f, 0.0f);

        isWalkingBackwards = false;
        isWalkingForwards = false;
        isWalkingLeft = false;
        isWalkingRight = false;
    }


    void Update(){
        if (health <= 0.0f){
            health = 0.0f;
            animationController.SetBool("isBackstepJump", false);
            animationController.SetBool("isJumpingForwards", false);
            animationController.SetBool("Idle", false);
            animationController.SetBool("isWalkingForwards", false);
            animationController.SetBool("isWalkingBackwards", false);
            animationController.SetBool("isWalkingLeft", false);
            animationController.SetBool("isWalkingRight", false);
            animationController.SetBool("isRunning", false);
            animationController.SetBool("isCrouch", false);

        }

        if (health <= 0.0f && !isDead){
            // DeathStat();
            Debug.Log("You died!");
        }

        else if (!isDead){
            if(has_won){
                Debug.Log("You won!");
            }
            else{
                Move();
            }
        }
        bool is_crouching = false;
        if ( (animationController.GetCurrentAnimatorStateInfo(0).IsName("isCrouch")))
            {
                is_crouching = true;
            }

        if (is_crouching)
            {
                GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.0f, GetComponent<CapsuleCollider>().center.z);
            }
        else
        {
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, 0.9f, GetComponent<CapsuleCollider>().center.z);
        }
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        Vector3 forwardMovement = new Vector3(xdirection, 0.0f, zdirection) * (isWalkingForwards || isWalkingBackwards ? 1 : 0);
        Vector3 lateralMovement = new Vector3(-zdirection, 0.0f, xdirection) * (isWalkingLeft || isWalkingRight ? -1 : 0);

        movementDirection = (forwardMovement + lateralMovement);


        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else if (verticalVelocity <= 0.0f){
                verticalVelocity = 0.0f;
            }
        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
        characterController.Move(movementDirection * velocity * Time.deltaTime);
        characterController.Move(verticalMovement * Time.deltaTime);
        
    }

    void Move(){
        if (health > 0.0f){
            animationController.SetBool("isBackstepJump", false);
            animationController.SetBool("isJumpingForwards", false);
            animationController.SetBool("Idle", false);
            animationController.SetBool("isWalkingForwards", false);
            animationController.SetBool("isWalkingBackwards", false);
            animationController.SetBool("isWalkingLeft", false);
            animationController.SetBool("isWalkingRight", false);
            animationController.SetBool("isRunning", false);
            animationController.SetBool("isCrouch", false);
            animationController.SetBool("CrouchForwards", false);
            animationController.SetBool("CrouchBackwards", false);
            animationController.SetBool("CrouchLeft", false);
            animationController.SetBool("CrouchRight", false);
            isWalkingBackwards = false;
            isWalkingForwards = false;
            isWalkingLeft = false;
            isWalkingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && characterController.transform.position.y < 0.8f) 

            {
                animationController.SetBool("isJumpingForwards", true);
                verticalVelocity = jumpForce;
                velocity += .5f;
                    if(velocity > walking_velocity*1.5){
                        velocity = walking_velocity*1.5f;
                    }
            }
        else if (Input.GetKeyDown(KeyCode.B)){
            animationController.SetBool("isBackstepJump", true);
            velocity -= .5f;
                if(velocity < -1.0f * walking_velocity*1.5f){
                    velocity = -1.0f * walking_velocity*1.5f;
                }
        }
        else{


            if (Input.GetKey(KeyCode.C) &&Input.GetKey(KeyCode.W))
            {
                animationController.SetBool("CrouchForwards", true);
                isWalkingForwards = true;
                velocity = crouchVelocity;
            }
            else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.S))
            {
                animationController.SetBool("CrouchBackwards", true);
                isWalkingBackwards = true;
                velocity = -1 * crouchVelocity;
            }
            else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.A))
            {
                animationController.SetBool("CrouchLeft", true);
                isWalkingLeft = true;
                velocity = -1 * crouchVelocity;
            }
            else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.D))
            {
                animationController.SetBool("CrouchRight", true);
                isWalkingRight = true;
                velocity =crouchVelocity;
            }

            else if (Input.GetKey(KeyCode.C))
            {
                animationController.SetBool("isCrouch", true);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                isWalkingForwards = true;
                animationController.SetBool("isRunning", true);
                velocity += .5f;
                if(velocity > runningVelocity){
                    velocity = runningVelocity;
                }
            }

            else if (Input.GetKey(KeyCode.W))
            {
                isWalkingForwards = true;
                animationController.SetBool("isWalkingForwards", true);
                velocity += .5f;
                if(velocity > walking_velocity){
                    velocity = walking_velocity;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                isWalkingBackwards = true;
                animationController.SetBool("isWalkingBackwards", true);
                velocity -= .5f;
                if(velocity < -1 * walking_velocity/1.5f){
                    velocity = -1 * walking_velocity/1.5f;
                }
            }

            else if (Input.GetKey(KeyCode.A)){
                isWalkingLeft = true;
                animationController.SetBool("isWalkingLeft", true);
                velocity -= .5f;
                if(velocity < -1 * walking_velocity){
                    velocity = -1 *walking_velocity;
                }
            }
            else if (Input.GetKey(KeyCode.D)){
                isWalkingRight = true;
                animationController.SetBool("isWalkingRight", true);
                velocity += .5f;
                if(velocity > walking_velocity){
                    velocity = walking_velocity;
                }
            }
            else{
                animationController.SetBool("Idle", true);
                velocity = 0f;
            }
        }
    }

    
}

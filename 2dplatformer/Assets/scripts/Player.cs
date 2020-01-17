using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    public float jumpHeight = 3.5f;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .05f;
    float accelerationTimeGrounded = .015f;
    float moveSpeed = 19f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;
    public bool canSlide;

    public float dashDistance;
    public float dashTime;
    float startDashTime = .1f;
    float elapsedTime = Mathf.Infinity;
    public bool dash;

    public bool sprinting;
    public float sprintMultiplier;

    float gravity;
    float jumpVelocity;
    Vector2 velocity;
    float velocityXSmoothing;
    [HideInInspector]
    public Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    
    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        print("Gravity: " + gravity + "   Jump Velocity:   " + jumpVelocity);

        dashTime = startDashTime;
        sprinting = false;
    }

    private void FixedUpdate()
    {
        
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;

            }
            else
            {
                velocity.y = 0;
            }
        }

        if (dash)
        {
            elapsedTime = 0f;
        }

        if (elapsedTime < dashTime)
        {           
            elapsedTime += Time.deltaTime;
            gravity = 0;
            controller.collisions.below = false; 
            OnDashInputDown();
        }
        else if (elapsedTime > dashTime)
        {
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); 
        }
        
        if (sprinting)
        {
            velocity.x = velocity.x * sprintMultiplier;
        }
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.collisions.below)
        {
            sprinting = true;
        } 
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            sprinting = false;
        }

    }

    public void SetDirectionalInput (Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputdown()
    {

        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }

        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) // not jumping against max slope
                {
                    velocity.y = jumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = jumpVelocity * controller.collisions.slopeNormal.x;
                }
                
            }
            velocity.y = jumpVelocity;
        }

    }

    public void OnDashInputDown()
    {   
         if (dash && controller.collisions.faceDir == 1)
         {
             controller.collisions.below = false;
             velocity.y = 0;
             velocity = Vector2.right * dashDistance;
             dash = false;
         }
         else if (dash && controller.collisions.faceDir == -1)
         {
             controller.collisions.below = false;
             velocity.y = 0;
             velocity = Vector2.left * dashDistance;
             dash = false;
         }       
    } 

    void HandleWallSliding()
    {
        
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && canSlide)
        {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

}



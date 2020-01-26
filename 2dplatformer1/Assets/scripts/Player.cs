using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{

    public float jumpHeight = 3.5f;
    public float timeToJumpApex = .4f;
    public int maxTimesJump = 2;
    [HideInInspector]
    public int timesJumped;
    public float consecutiveJumpMultiplier;
    [HideInInspector]
    public float jumpElapsedTime = Mathf.Infinity;
    [HideInInspector]
    public float protectedJumpTime = 0.2f;
    [HideInInspector]
    public bool canJump;
    [HideInInspector]
    public bool hasJumped;
    [HideInInspector]
    public float protectetJumpAfterSlideTime = 0.7f;
    [HideInInspector]
    public float jumpAfterSlideElapsedTime = Mathf.Infinity;
    [HideInInspector]
    public bool hasWallJumped;

    float accelerationTimeAirborne = .05f;
    float accelerationTimeGrounded = .015f;
    float moveSpeed = 19f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;
    [HideInInspector]
    public bool canSlide;

    public float dashDistance;
    public float dashTime;
    float startDashTime = .1f;
    float elapsedTime = 0;
    [HideInInspector]
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
    [HideInInspector]
    public bool wallSliding;

    int wallDirX;
    [HideInInspector]
    public bool facingRight;
    
    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        print("Gravity: " + gravity + "   Jump Velocity:   " + jumpVelocity);

        hasJumped = false;
        canJump = true;
        timesJumped = 0;
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

        TimingDashLength();

        TimeJumpAllow();

        Sprinting();

        ProtectJumpCount();

        HandleJumpCountReset();

        Flip();
    }

    void TimingDashLength()
    {
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
    }

    void TimeJumpAllow()
    {
        if (hasJumped)
        {
            canJump = false;
            jumpElapsedTime += Time.deltaTime;

            if (jumpElapsedTime >= protectedJumpTime)
            {
                canJump = true;
            }
        }
    }

    void ProtectJumpCount()
    {
        if (hasWallJumped)
        {
            canJump = false;
            jumpAfterSlideElapsedTime += Time.deltaTime;

            if (jumpAfterSlideElapsedTime >= protectetJumpAfterSlideTime)
            {
                canJump = true;
            }
        }
    }

    void Sprinting()
    {
        if (sprinting)
        {
            velocity.x = velocity.x * sprintMultiplier;
        }
    }
    public void Flip()
    {
        if (controller.collisions.faceDir == 1)
        {
            gameObject.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            facingRight = false;
        }
    }

    public void HandleJumpCountReset()
    {
        if (controller.collisions.below && (timesJumped != 0) || wallSliding)
        {
            timesJumped = 0;
        }
    }

    public void SetDirectionalInput (Vector2 input)
    {
        directionalInput = input;
    }

    public void WallJumping()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                hasWallJumped = true;
                jumpAfterSlideElapsedTime = 0;
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                hasWallJumped = true;
                jumpAfterSlideElapsedTime = 0;
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                hasWallJumped = true;
                jumpAfterSlideElapsedTime = 0;
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
    }
    public void OnJumpInputdown()
    {

        if ((timesJumped < maxTimesJump)  && canJump)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    if (!wallSliding)
                    {
                        timesJumped += 1;
                        hasJumped = true;
                        jumpElapsedTime = 0;
                    }

                    velocity.y = jumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = jumpVelocity * controller.collisions.slopeNormal.x;                    
                }
            }
            else if (timesJumped > 1)
            {
                if (!wallSliding)
                {
                    timesJumped += 1;
                    hasJumped = true;
                    jumpElapsedTime = 0;
                }

                velocity.y = jumpVelocity * consecutiveJumpMultiplier;
            }
            else
            {
                if (!wallSliding)
                {
                    timesJumped += 1;
                    hasJumped = true;
                    jumpElapsedTime = 0;
                }

                velocity.y = jumpVelocity;               
            }            
        }
    }

    public void OnDashInputDown()
    {   
         if (dash && controller.collisions.faceDir == 1)
         {
            if (!controller.collisions.slidingDownMaxSlope)
            {
                controller.collisions.below = false;
                velocity.y = 0;
                velocity = Vector2.right * dashDistance;
                dash = false;
            }
            else if (controller.collisions.slidingDownMaxSlope)
            {
                controller.collisions.below = false;
                velocity.y = 0;
                velocity = Vector2.left * dashDistance;
                dash = false;
            }            
         }
         else if (dash && controller.collisions.faceDir == -1)
         {
            if (!controller.collisions.slidingDownMaxSlope)
            {
                controller.collisions.below = false;
                velocity.y = 0;
                velocity = Vector2.left * dashDistance;
                dash = false;
            }
            else if (controller.collisions.slidingDownMaxSlope)
            {
                controller.collisions.below = false;
                velocity.y = 0;
                velocity = Vector2.right * dashDistance;
                dash = false;
            }
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
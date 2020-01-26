using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (player.wallSliding && (Input.GetKeyUp(KeyCode.Space)))
        {
            player.WallJumping();
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.canJump && !player.wallSliding)
        {
            player.OnJumpInputdown();            
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.dash = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.controller.collisions.below)
        {
            player.sprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            player.sprinting = false;
        }

    }
}

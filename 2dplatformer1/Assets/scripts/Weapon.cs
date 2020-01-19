using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;

    private void FixedUpdate()
    {
        Flip();
    }

    void Flip()
    {
        if(player.facingRight)
        {
            transform.eulerAngles = new Vector3( 0 , 0 , 0 );
        } else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Destroyable thing = collision.GetComponent<Destroyable>();
            if (thing != null)
            {
                thing.DestroySelf();
            }
            
        }
    }
}

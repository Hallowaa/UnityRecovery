using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSticking : MonoBehaviour
{
    public PlayerMovement pm;

    private void Awake()
    {
        pm = GetComponentInParent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rocks"))
        {
            pm.canStickToWall = false;
        }

        switch (other.gameObject.layer)
        {
            case 9:
                if (!pm.isGrounded() && pm.highEnoughCheck() && pm.canStickToWall)
                {
                    pm.isStickingToWall = true;
                }
                break;
            case 16:
                if (!pm.isGrounded() && pm.highEnoughCheck() && pm.canStickToWall)
                {
                    pm.isStickingToWall = true;
                }
                break;
            default:
                pm.isStickingToWall = false;
                break;
        }
    }
}

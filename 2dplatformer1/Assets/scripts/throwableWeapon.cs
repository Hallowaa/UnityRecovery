using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableWeapon : MonoBehaviour
{
    public float speed = 50f;

    Vector2 prevPos;
    Vector3 origin;
    Vector3 maxRange;

    bool neverDone = true;

    public LayerMask layer;

    Throwable throwable;
    Spin spin;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
        origin = transform.position;
        maxRange = new Vector3(300f, 300f, 0f);

        spin = GetComponentInChildren<Spin>();
        throwable = FindObjectOfType<Throwable>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        prevPos = transform.position;
        Vector2 moveAmount = Vector2.right * speed;
        transform.Translate(moveAmount * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Linecast(new Vector2(prevPos.x, prevPos.y), new Vector2(transform.position.x, transform.position.y));

        Debug.DrawLine(new Vector2(prevPos.x, prevPos.y), new Vector2(transform.position.x, transform.position.y));

        if (hit)
        {
            if(hit.collider.GetComponent<Destroyable>())
            {
                Destroyable destroyable = hit.collider.GetComponent<Destroyable>();
                destroyable.DestroySelf();
                Destroy(gameObject);
                throwable.stuckShurikens -= 1;
            }

            if (hit.collider.IsTouchingLayers(layer));
            {
                speed = 0f;
                spin.spins = false;
                if (neverDone)
                {
                    throwable.stuckShurikens += 1;
                    neverDone = false;
                }
            }
        }

        if (Mathf.Abs(transform.position.x - origin.x)> maxRange.x)
        {
            Destroy(gameObject);
        }
        if (Mathf.Abs(transform.position.y - origin.y) > maxRange.y)
        {
            Destroy(gameObject);
        }
    }
}
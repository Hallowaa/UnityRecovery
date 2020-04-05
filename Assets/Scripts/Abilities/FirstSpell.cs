using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSpell : MonoBehaviour
{
    public LayerMask hittable;
    public GameObject collisionCompensation;

    public float speed = 20f;
    public float lifeTime = 45f;

    Vector3 oldPos;
    RaycastHit hit;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        oldPos = transform.position;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        PerformLinecast();

        if (hit.collider)
        {
            speed = 0f;
            transform.position = hit.point - (transform.forward * 0.8f);
        }

        Lifetime();

    }

    void PerformLinecast()
    {
        if (speed > 0.5f)
        {
            Physics.Linecast(oldPos, collisionCompensation.transform.position, out hit, hittable);
        }
    }

    void Lifetime()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject collisionCompensation;
    public GameObject sphere;
    public float speed = 45f;
    Vector3 oldPos;
    RaycastHit hit;
    public LayerMask hittable;
    public bool hasHit = false;
    public bool shrink = false;

    public float sizeBeforeLaunch = 5f;
    public float sizeBeforeShrink = 20f;
    public float sizeBeforeExplosion = 1.5f;


    Vector3 scaleChange;
    public Vector3 hitPoint;
    private void Start()
    {
        scaleChange = new Vector3(0.5f, 0.5f, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        oldPos = transform.position;

        ChargeUp();
        if(sphere.transform.localScale.x >= sizeBeforeLaunch)
        { 
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (hit.collider)
            {
                speed = 0f;
                hitPoint = hit.point;
                transform.position = hit.point;
                hasHit = true;
                if (sphere.transform.localScale.x >= sizeBeforeShrink)
                {
                    shrink = true;
                }               
            }
            if (speed > 0.5f)
            {
                Physics.Linecast(collisionCompensation.transform.position, oldPos, out hit, hittable);
            }
        }
        ShrinkBeforeExplode();
        Debug.DrawLine(collisionCompensation.transform.position, oldPos, Color.red);
    }

    void ChargeUp()
    {
        if (sphere.transform.localScale.x <= sizeBeforeLaunch && !shrink)
        {
            transform.position = Singleton.Instance.throwPoint.transform.position;
            transform.rotation = Singleton.Instance.throwPoint.transform.rotation;
        }
    }

    void ShrinkBeforeExplode()
    {
        if (sphere.transform.localScale.x <= sizeBeforeExplosion && shrink)
        {
            RasenShurikenExplosionInstantiate();
            Destroy(gameObject);
        }
    }

    public void RasenShurikenExplosionInstantiate()
    {
        GameObject RasenExplosion = Instantiate(Singleton.Instance.rasenExplosion, hitPoint, Quaternion.identity) as GameObject;
    }

}

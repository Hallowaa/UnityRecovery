using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasenShuriken : MonoBehaviour
{

    public float rotationSpeed = 720f;
    Throw rasenShurikenParent;
    Vector3 scaleChange;


    // Start is called before the first frame update
    void Start()
    {
        rasenShurikenParent = GetComponentInParent<Throw>();
        scaleChange = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        ScaleUp(30f,60f, scaleChange,scaleChange * 6);
    }

    void ScaleUp(float scaleTrheshold, float explosionThreshold, Vector3 scaleChange, Vector3 explosionScaleChange)
    {
        if (transform.localScale.x < scaleTrheshold)
        {
            transform.localScale += scaleChange * Time.deltaTime;
        }

        if (transform.localScale.x >= scaleTrheshold && transform.localScale.x <= explosionThreshold && rasenShurikenParent.hasHit && !rasenShurikenParent.shrink)
        {
            transform.localScale += explosionScaleChange * Time.deltaTime;
        }

        if (transform.localScale.x >= 1.5f && rasenShurikenParent.shrink)
        {
            transform.localScale -= scaleChange * 12 * Time.deltaTime;
        }
    }

}

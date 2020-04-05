using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    Vector3 upScale;
    bool shrink;
    float timeMax = 1.5f;
    float timePassed = 500f;
    // Start is called before the first frame update
    void Start()
    {
        upScale = new Vector3(120f, 120f, 120f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x <= 50f && !shrink)
        {
            transform.localScale += upScale * Time.deltaTime;
            timePassed = 0f;
        }
        if (timePassed < timeMax)
        {
            timePassed += Time.deltaTime;
        }
        if (timePassed >= timeMax)
        {
            ScaleDown();
        }       
    }

    void ScaleDown()
    {
        if(transform.localScale.x >= 0f)
        {
            shrink = true;
            transform.localScale -= upScale * 0.8f * Time.deltaTime;
        }
        else if (transform.localScale.x <= 0f)
        {
            Destroy(gameObject);
        }
       
    }
}

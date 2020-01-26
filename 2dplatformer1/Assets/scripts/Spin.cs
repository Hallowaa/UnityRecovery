using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool spins;
    

    // Start is called before the first frame update
    void Start()
    {
        spins = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spins)
        {
            transform.Rotate(0f, 0f, 720 * Time.deltaTime, Space.Self);
        }
       
    }
}

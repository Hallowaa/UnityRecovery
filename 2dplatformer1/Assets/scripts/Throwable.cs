using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public Transform throwPoint;
    public GameObject throwable;
    public Player player;

    int maxStuckShurikens = 7;
    public List<GameObject> shurikens;
    public int stuckShurikens;

    // Start is called before the first frame update
    void Start()
    {
        shurikens = new List<GameObject>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Throw();
        }
        CheckShurikenAmount(shurikens);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Flip();
    }
    void Flip()
    {
        if (player.facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {            
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void Throw()
    {
        
        GameObject newShuriken = Instantiate(throwable, throwPoint.position, throwPoint.rotation) as GameObject;
        shurikens.Add(newShuriken);
    }

    void CheckShurikenAmount(List<GameObject> shurikens)
    {
        if (stuckShurikens >= maxStuckShurikens)
        {
            GameObject.Destroy(shurikens[0]);
            shurikens.RemoveAt(0);
            stuckShurikens -= 1;
        }

        for (int i = 0; i < shurikens.Count; i++)
        {
            if (shurikens[i] == null)
            {
                shurikens.RemoveAt(i);
            }
        }
    }
}

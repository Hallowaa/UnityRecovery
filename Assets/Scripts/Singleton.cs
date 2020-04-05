using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public PlayerMovement player;

    public Eyes playerCamera;

    public GameObject shootingPoint;

    public GameObject throwPoint;

    public GameObject shuriken;

    public GameObject rasenShuriken;

    public GameObject rasenExplosion;

    public static Singleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindObjectOfType<PlayerMovement>();
        playerCamera = player.GetComponentInChildren<Eyes>();
        shootingPoint = GameObject.Find("Shooting Point");
        throwPoint = GameObject.Find("Throw Point");
    }

    public void FirstSpellInstantiate()
    {
        GameObject Shuriken = Instantiate(shuriken, shootingPoint.transform.position, playerCamera.transform.rotation) as GameObject;
    }

    public void RasenShurikenInstatiate()
    {
        GameObject RasenShuriken = Instantiate(rasenShuriken, throwPoint.transform.position, playerCamera.transform.rotation) as GameObject;
    }




}

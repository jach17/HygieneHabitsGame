using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;
    //[SerializeField]
    private Level3Manager level;

    private GameObject[] dishesGameObjects;
    private Transform[] splats;
    private bool canDelete;

    public bool CanDelete
    {
        get { return canDelete; }
        set { canDelete = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //level = gameManager.GetComponent<Level3Manager>();
        //level = FindObjectOfType<Level3Manager>();
        splats = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            splats[i] = transform.GetChild(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.childCount);
        if (transform.childCount == 0)
        {
            //Debug.Log("AAAAAAAA");
            canDelete = true;
            //level.NextDish();
            //Destroy(gameObject);
        }
        //Transform[] splats = GetComponentsInChildren<Transform>();
        /*if (splats.Length == 0)
        {
            Debug.Log("AAAAAAAA");
            canDelete = true;
        }*/
        /*foreach (Transform splat in splats)
        {
            if (splat != null)
            {
                return;
            }
            Console.WriteLine("BORRAR!!!");
            canDelete = true;
          }*/
    }
}

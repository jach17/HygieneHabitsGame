using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashController : MonoBehaviour
{
  //[SerializeField]
  //private Level3Manager level;
    //[SerializeField]
    //private Vector2 valuesLoc;

    //private Draggable draggable;
    private void Start()
    {
        //draggable = GetComponent<Draggable>();
        //level = FindObjectOfType<Level3Manager>();
    }

    private void Update()
    {
        //if (level.MoveRandom)
        //{
            //draggable.CanDrag = false;
            //transform.position = new Vector2(Random.Range(-valuesLoc.x, valuesLoc.x), Random.Range(-valuesLoc.y, valuesLoc.y));
            
        //}
        //draggable.CanDrag = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Splat")
        {
            Destroy(collision.gameObject);
        }
    }
}

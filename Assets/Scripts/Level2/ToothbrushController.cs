using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothbrushController : MonoBehaviour
{
    [SerializeField]
    private Level2Mini1 level2;

    private void Start()
    {
        level2 = FindObjectOfType<Level2Mini1>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Virus")
        {
            level2.AddPunto();
            Destroy(collision.gameObject);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapController : MonoBehaviour
{
    [SerializeField]
    private Level2Min2Manager level2;

    private void Start()
    {
        level2 = FindObjectOfType<Level2Min2Manager>();
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

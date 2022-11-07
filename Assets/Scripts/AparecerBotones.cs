using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerBotones : MonoBehaviour
{
    [SerializeField]
    private GameObject boton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Tommy")
        {
            boton.SetActive(true);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tommy")
        {
            boton.SetActive(false);
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarSonido : MonoBehaviour
{
   [SerializeField] private GameObject Sonido;
    // Start is called before the first frame update
    public void ActivarDesactivarSonido()
    {
        if (Sonido.active == true)
        {
            Sonido.SetActive(false);
        }
        else{
            Sonido.SetActive(true);
        }
    }
}

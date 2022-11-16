using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    [SerializeField]
    private GameObject aviso;
    public void Exit()
    {
        Application.Quit();
    }
    public void ActivarAvisoSalida()
    {
        aviso.SetActive(true);
    }
    public void DesactivarAvisoSalida()
    {
        aviso.SetActive(false);
    }

}

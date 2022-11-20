using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    [SerializeField]
    private GameObject aviso, boton1, boton2;
    public void Exit()
    {
        Application.Quit();
    }
    public void ActivarAvisoSalida()
    {
        aviso.SetActive(true);
        boton1.SetActive(false);
        boton2.SetActive(false);
    }
    public void DesactivarAvisoSalida()
    {
        aviso.SetActive(false);
        boton1.SetActive(true);
        boton2.SetActive(true);
    }

}

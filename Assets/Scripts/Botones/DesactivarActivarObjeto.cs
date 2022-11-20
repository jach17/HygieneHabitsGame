using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarActivarObjeto : MonoBehaviour
{
    [SerializeField]
    private GameObject objeto;
    public void ActivarObjeto()
    {
        objeto.SetActive(true);
    }
    public void DesactivarObjeto()
    {
        objeto.SetActive(false);
    }
}

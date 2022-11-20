using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoTemporal : MonoBehaviour
{
    [SerializeField]
    private GameObject objeto;
    [SerializeField]
    private float tiempodevida;
    private void Start()
    {
        Destroy(objeto, tiempodevida);
    }
}

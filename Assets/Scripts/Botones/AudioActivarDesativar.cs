using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioActivarDesativar : MonoBehaviour
{
    [SerializeField] private AudioSource Sonido;
    // Start is called before the first frame update
    public void ActivarSonido()
    {
        Sonido.Play();
    }
    public void DesactivarSonido()
    {
        Sonido.Pause();
    }
}

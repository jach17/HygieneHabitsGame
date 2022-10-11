using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainForm;
    [SerializeField]
    private GameObject signUpForm;
    
    private string user,password;
    //[SerializeField]
    //private Button btnPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodo que se llama cuando se pulsa el boton de play
    public void Play()
    {
        Debug.Log("Play");
    }

    //Metodo que se llama cuando se pulsa el boton de Crear cuenta
    public void SignUp()
    {
        Debug.Log("SignUp");

    }

    //Metodo que se ejecuta cuando se pulsa el boton de enviar en el formulario de registro
    public void Send()
    {
        Debug.Log("Send");

    }

    //Metodo para obtener la informacion del usuario guardada en el telefono, si ya existe la cuenta se salta la escena de login
    public void GetAccount()
    {
        Debug.Log("GetAccount");

    }

    //Metodo que se llama cuando se deja de escribir en el txt de usuario
    public void ReadUser(string s)
    {
        user = s;
    }

    //Metodo que se llama cuando se deja de escribir en el txt de la contraseña
    public void ReadPassword(string s)
    {
        password = s;
    }
}

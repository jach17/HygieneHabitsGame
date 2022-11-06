using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private GameObject servicesGameObject;

    private Services services;

    private string namePlayer;
    private string password;
    private bool reg;
    // Start is called before the first frame update
    void Start()
    {
        services = servicesGameObject.GetComponent<Services>();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        namePlayer = PlayerPrefs.GetString("namePlayer");
        password = PlayerPrefs.GetString("password");
        if (namePlayer != "" && password != "")
        {
            services.PostAuth(namePlayer, password);
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }
}


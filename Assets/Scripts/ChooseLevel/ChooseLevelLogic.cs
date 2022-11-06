using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChooseLevelLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject servicesGameObject;

    private Services services;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteKey("dateEnd");
        services = servicesGameObject.GetComponent<Services>();
        
        if (PlayerPrefs.GetString("activeSesion") == "")
        {
            services.PostSesion(DateTime.Now.ToString().Replace("/", "-"), "");
            PlayerPrefs.SetString("activeSesion", "true");
            PlayerPrefs.Save();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }

    public void SignOut()
    {
        PlayerPrefs.DeleteKey("namePlayer");
        PlayerPrefs.DeleteKey("password");
        SceneManager.LoadScene("LoginScene");
    }
}

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
            StartCoroutine(CheckInternetPostSesion_Coroutine());
            
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
        PlayerPrefs.DeleteKey("statusLevel1");
        PlayerPrefs.DeleteKey("statusLevel2");
        PlayerPrefs.DeleteKey("statusLevel3");
        PlayerPrefs.DeleteKey("statusLevel4");
        PlayerPrefs.DeleteKey("statusLevel5");
    }

    IEnumerator CheckInternetPostSesion_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("postsesion");
            services.PostSesion(DateTime.Now.ToString().Replace("/", "-"), "");
            PlayerPrefs.SetString("activeSesion", "true");
            PlayerPrefs.Save();
        }
    }
}

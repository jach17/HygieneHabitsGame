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

    [SerializeField]
    private GameObject image1;

    [SerializeField]
    private GameObject image2;
    [SerializeField]
    private GameObject image3;
    [SerializeField]
    private GameObject image4;
    [SerializeField]
    private GameObject image5;

    private Services services;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteKey("dateEnd");
        services = servicesGameObject.GetComponent<Services>();
        LevelCheck();
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

    public void LevelCheck()
    {

        if (PlayerPrefs.GetInt("statusLevel1") == 0)
        {
            image1.SetActive(true);
        }
        else
        {
            image1.SetActive(false);
        }
        if (PlayerPrefs.GetInt("statusLevel2") == 0)
        {
            image2.SetActive(true);
        }
        else
        {
            image2.SetActive(false);
        }

        if (PlayerPrefs.GetInt("statusLevel3") == 0)
        {
            image3.SetActive(true);
        }
        else
        {
            image3.SetActive(false);
        }

        if (PlayerPrefs.GetInt("statusLevel4") == 0)
        {
            image4.SetActive(true);
        }
        else
        {
            image4.SetActive(false);
        }

        if (PlayerPrefs.GetInt("statusLevel5") == 0)
        {
            image5.SetActive(true);
        }
        else
        {
            image5.SetActive(false);
        }
    }
}

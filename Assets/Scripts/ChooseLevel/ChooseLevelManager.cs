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
using static UnityEngine.EventSystems.EventTrigger;

public class ChooseLevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject image;
    private void Start()
    {

    }

    [SerializeField] private GameObject m_MenuPausa, m_Pausa;
    public void PlayHome()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
    public void PlayLeve1()
    {
        //LevelDirection.Level = "Level1";
        //StartCoroutine(CheckInternet_Coroutine("Level1"));
        SceneManager.LoadScene("Level1");
    }
    public void PlayLevel11()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level1-1"));
        SceneManager.LoadScene("Level1-1");
    }
    public void PlayLevel1_2()
    {
        //LevelDirection.Level = "Level1-1";
        
        //StartCoroutine(CheckInternet_Coroutine("Level1-2"));
        SceneManager.LoadScene("Level1-2");
    }
    public void PlayLevel2_1()
    {
        //LevelDirection.Level = "Level1-1";
       // StartCoroutine(CheckInternet_Coroutine("Level2-1"));
        SceneManager.LoadScene("Level2-1");
    }
    public void PlayLevel2_2()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level2-2");
    }
    public void PlayLevel2_3()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level2-3");
    }

    public void PlayLevel3_1()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level3-1");
    }
    public void PlayLeve2()
    {
        
        
            if (PlayerPrefs.GetInt("statusLevel2") == 0)
            {
                image.SetActive(true);
                StartCoroutine(Esperar());
                //image.SetActive(false);
            }
            else
            {
                StartCoroutine(CheckInternet_Coroutine("Level2"));
            }
           
        

    }
    public void DetenerTiempo()
    {
        Time.timeScale = 0f;
    }
    public void ActivarTiempo()
    {
        Time.timeScale = 1f;
    }
    public void PlayLeve3()
    {
        if (PlayerPrefs.GetInt("statusLevel3")==0)
        {
            image.SetActive(true);
            StartCoroutine(Esperar());
            //image.SetActive(false);
        }
        else
        {
            StartCoroutine(CheckInternet_Coroutine("Level3"));
        }

    }
    public void PlayLeve31()
    {
        //LevelDirection.Level = "Level3";
        //SceneManager.LoadScene("LoadingScene");
        if (PlayerPrefs.GetInt("statusLevel3") == 1)
        {
            StartCoroutine(CheckInternet_Coroutine("Level31"));
        }

    }
    public void PlayLeve2Prueba()
    {
        SceneManager.LoadScene("Level2");
    }
    public void PlayLeve3Prueba()
    {
        SceneManager.LoadScene("Level3");
    }
    public void PlayLeve4Prueba()
    {
        SceneManager.LoadScene("Level4");
    }
    public void PlayLeve5Prueba()
    {
        SceneManager.LoadScene("Level5");
    }
    public void PlayLeve4()
    {
        if (PlayerPrefs.GetInt("statusLevel4")==0)
        {
            image.SetActive(true);
            StartCoroutine(Esperar());
            //image.SetActive(false);
        }
        else
        {
            StartCoroutine(CheckInternet_Coroutine("Level4"));
        }
    }
    public void PlayLevel4_1()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level4-1");
    }
    public void PlayLevel4_0_2()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level4.0.2");
    }
    public void PlayLevel4_2()
    {
        //LevelDirection.Level = "Level1-1";
        //StartCoroutine(CheckInternet_Coroutine("Level2-2"));
        SceneManager.LoadScene("Level4-2");
    }
    public void PlayLeve5()
    {
        if (PlayerPrefs.GetInt("statusLevel5")==0)
        {
            image.SetActive(true);
            StartCoroutine(Esperar());
            //image.SetActive(false);
        }
        else
        {
            StartCoroutine(CheckInternet_Coroutine2("Level5"));
        }
    }
    public void PlayLeve5_1()
    {
        //LevelDirection.Level = "Level5-2";
        SceneManager.LoadScene("Level5_1");
    
    //StartCoroutine(CheckInternet_Coroutine("Level5-1"));
}

    IEnumerator CheckInternet_Coroutine(string scene)
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
            LevelDirection.Level = scene;
            SceneManager.LoadScene("LoadingScene");
        }
    }
    IEnumerator CheckInternet_Coroutine2(string scene)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene2");
        }
        else
        {
            LevelDirection.Level = scene;
            SceneManager.LoadScene("LoadingScene2");
        }
    }
    IEnumerator Esperar()
    {

        yield return new WaitForSeconds(4f);
        image.SetActive(false);
    }
}


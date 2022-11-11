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

public class ChooseLevelManager : MonoBehaviour
{
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
        SceneManager.LoadScene("Level1");
    }
    
    public void PlayLeve2()
    {
        if (PlayerPrefs.GetInt("statusLevel2") == 1)
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            Debug.Log(PlayerPrefs.GetInt("statusLevel2"));
            Debug.Log("BLOQUEADO");
        }
        
    }

    public void PlayLeve3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void PlayLeve2Prueba()
    {
        SceneManager.LoadScene("Level2");
    }
    public void PlayLeve4()
    {
        SceneManager.LoadScene("Level4-1");
    }
    public void PlayLeve5()
    {
        SceneManager.LoadScene("Level5-2");
    }

    
}


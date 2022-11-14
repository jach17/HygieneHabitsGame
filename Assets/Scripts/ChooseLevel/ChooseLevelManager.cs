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
        LevelDirection.Level = "Level1";
        SceneManager.LoadScene("LoadingScene");
    }
    public void PlayLevel11()
    {
        LevelDirection.Level = "Level1-1";
        SceneManager.LoadScene("LoadingScene");
    }
    public void PlayLeve2()
    {
        if (PlayerPrefs.GetInt("statusLevel2") == 1)
        {
            LevelDirection.Level = "Level2";
            SceneManager.LoadScene("LoadingScene");
        }
        
    }

    public void PlayLeve3()
    {
        if (PlayerPrefs.GetInt("statusLevel3") == 1)
        {
            LevelDirection.Level = "Level3";
            SceneManager.LoadScene("LoadingScene");
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
        LevelDirection.Level = "Level4";
        SceneManager.LoadScene("LoadingScene");
    }
    public void PlayLeve5()
    {
        LevelDirection.Level = "Level5";
        SceneManager.LoadScene("LoadingScene");
    }
    public void PlayLeve5_2()
    {
        LevelDirection.Level = "Level5-2";
        SceneManager.LoadScene("LoadingScene");
    }

}


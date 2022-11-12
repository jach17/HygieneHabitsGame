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
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private GameObject servicesGameObject;

    [SerializeField]
    private Button btnTry;

    private Services services;

    private float time = 7f;
    private string namePlayer;
    private string password;
    private bool reg;
    private bool timeFinished;
    // Start is called before the first frame update
    void Start()
    {

        if (LevelDirection.Level == null)
        {
            services = servicesGameObject.GetComponent<Services>();
            StartCoroutine(Wait());
        }
        else
        {
            StartCoroutine(WaitForLevel());
        }

    }

    private void Update()
    {
        if (LevelDirection.Level == "LoadingScene")
        {
            if (time <= 0 && !timeFinished)
            {
                StopAllCoroutines();
                btnTry.gameObject.SetActive(true);
                timeFinished = true;
            }
            time -= Time.deltaTime;
        }

    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
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

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSeconds(1.5f);
        switch (LevelDirection.Level)
        {
            case "Level1":
                SceneManager.LoadScene("Level1");
                break;
            case "Level1-1":
                SceneManager.LoadScene("Level1-1");
                break;
            case "Level2":
                SceneManager.LoadScene("Level2");
                break;
            case "Level2-1":
                SceneManager.LoadScene("Level2-1");
                break;
            case "Level2-2":
                SceneManager.LoadScene("Level2-2");
                break;
            case "Level3":
                SceneManager.LoadScene("Level3");
                break;
            case "Level3-1":
                SceneManager.LoadScene("Level3-1");
                break;
            case "Level4":
                SceneManager.LoadScene("Level4");
                break;
            case "Level4-1":
                SceneManager.LoadScene("Level4-1");
                break;
            case "Level5":
                SceneManager.LoadScene("Level5");
                break;
            case "Level5-1":
                SceneManager.LoadScene("Level5-1");
                break;
            case "Level5-2":
                SceneManager.LoadScene("Level5-2");
                break;
        }
    }

    public void Connect()
    {
        time = 6f;
        btnTry.gameObject.SetActive(false);
        timeFinished = false;
        services.PostAuth(namePlayer, password);

    }
}


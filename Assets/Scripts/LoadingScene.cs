using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    async void Start()
    {

        if (LevelDirection.Level == null)
        {
            services = servicesGameObject.GetComponent<Services>();

            //StartCoroutine(Wait());
            //StartCoroutine(CheckInternet_Coroutine());
            await CheckInternet_Async();

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

    private void OnApplicationQuit()
    {
        if (LevelDirection.Level != null)
        {
            PlayerPrefs.DeleteKey("activeSesion");
            PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
            PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
        }

    }

    IEnumerator CheckInternet_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            request.Dispose();
            Debug.Log("Error de conexion");
            StopAllCoroutines();
            btnTry.gameObject.SetActive(true);
            timeFinished = true;
        }
        else
        {
            request.Dispose();
            Debug.Log("Conexion");
            StartCoroutine(Wait());
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

    async Task CheckInternet_Async()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield();
        }

        if (request.error != null)
        {
            request.Dispose();
            Debug.Log("Error de conexion");
            StopAllCoroutines();
            btnTry.gameObject.SetActive(true);
            timeFinished = true;
        }
        else
        {
            request.Dispose();
            Debug.Log("Conexion");
            await Wait2();
        }
    }

    async Task Wait2()
    {
        await Task.Delay(1000);

        namePlayer = PlayerPrefs.GetString("namePlayer");
        password = PlayerPrefs.GetString("password");
        if (namePlayer != "" && password != "")
        {
            //services.PostAuth(namePlayer, password);
            await services.PostAuthPlayer_Async(namePlayer, password);
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
        StopAllCoroutines();
        time = 7f;
        btnTry.gameObject.SetActive(false);
        timeFinished = false;
        //services.PostAuth(namePlayer, password);
        StartCoroutine(CheckInternet_Coroutine());

    }


}


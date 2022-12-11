using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

public class Level1Minigame : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtPoints;
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private TMP_Text txtPointsWin;

    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private float spawnWait;
    [SerializeField]
    private float startWait;
    [SerializeField]
    private Vector3 spawnValues;
    [SerializeField]
    private GameObject[] hazards;

    private int points;
    [SerializeField]
    private float time;
    [SerializeField]
    private float maxPoints = 50;
    [SerializeField]
    private float minPoints = 15;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject ServicesGameObject;
    Services services;

    [SerializeField]
    private GameObject winMenu;

    [SerializeField]
    private GameObject loseMenu;

    private bool levelFinished;
    // Start is called before the first frame update
    void Start()
    {
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        StartCoroutine(SpawnWaves());
        
    }

    // Update is called once per frame
    async void Update()
    {
        if (levelFinished)
        {
            
            return;
        }
        /*if (points == maxPoint && !levelFinished)
        {
            StopAllCoroutines();
            StartCoroutine(CheckInternetWin_Coroutine());
            return;
        }*/

        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            if (points>= minPoints)
            {
                Debug.Log("points>=");
                //StartCoroutine(CheckInternetWin_Coroutine());
                await CheckInternetWin_Async();
                winMenu.SetActive(true);
            }
            else
            {
                Debug.Log("lose");
                //StartCoroutine(CheckInternetLose_Coroutine());
                await CheckInternetLose_Async();
                loseMenu.SetActive(true);
            }
            return;
        }
        else
        {
            time -= Time.deltaTime;
        }
        txtTime.text = time.ToString("F2");
    }

    /*private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }*/
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
            PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
        }

    }
    public void AddPoint()
    {
        if (points >= maxPoints)
        {
            return;
        }
        points++;
        audioSource.Play();
        txtPoints.text = "Puntos: " + points.ToString();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(spawnValues.x,Random.Range( -spawnValues.y,spawnValues.y), spawnValues.z);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }

    IEnumerator CheckInternetWin_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            request.Dispose();
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            request.Dispose();
            levelFinished = true;

            services.PostReport(points.ToString(), 1);
            txtPointsWin.text = "Puntuación: " + points.ToString();
            winMenu.SetActive(true);

            if (PlayerPrefs.GetInt("statusLevel2") == 0)
            {
                PlayerPrefs.SetInt("statusLevel2", 1);
                services.UpdateLevelStatus("2");
            }
        }
    }

    async Task CheckInternetWin_Async()
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
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            request.Dispose();
            levelFinished = true;

            //services.PostReport(points.ToString(), 1);
            await services.PostReport_Aync(points.ToString(), 1);
            txtPointsWin.text = "Puntuación: " + points.ToString();
            winMenu.SetActive(true);

            if (PlayerPrefs.GetInt("statusLevel2") == 0)
            {
                PlayerPrefs.SetInt("statusLevel2", 1);
                //services.UpdateLevelStatus("2");
                await services.UpdateLevelStatus_Async("2");
            }
        }
    }

    IEnumerator CheckInternetLose_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            request.Dispose();
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            request.Dispose();
            time = 0;
            levelFinished = true;

            //services.PostReport(points.ToString(), 1);
          
            loseMenu.SetActive(true);
        }
    }

    async Task CheckInternetLose_Async()
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
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            request.Dispose();
            time = 0;
            levelFinished = true;

            //services.PostReport(points.ToString(), 1);
            await services.PostReport_Aync(points.ToString(), 1);
            loseMenu.SetActive(true);
        }
    }
}

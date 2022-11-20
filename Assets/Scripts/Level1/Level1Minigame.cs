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
    private float maxPoint = 15;

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
    void Update()
    {
        if (levelFinished)
        {
            
            return;
        }
        if (points == maxPoint && !levelFinished)
        {
            StopAllCoroutines();
            StartCoroutine(CheckInternetWin_Coroutine());
            return;
        }

        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            StartCoroutine(CheckInternetLose_Coroutine());
            if (points>= maxPoint)
            {
                winMenu.SetActive(true);
            }
            else
            {
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

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }

    public void AddPoint()
    {
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
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            levelFinished = true;

            services.PostReport(points.ToString(), 1);
            txtPointsWin.text = "Puntuaci�n: " + points.ToString();
            winMenu.SetActive(true);

            if (PlayerPrefs.GetInt("statusLevel2") == 0)
            {
                PlayerPrefs.SetInt("statusLevel2", 1);
                services.UpdateLevelStatus("2");
            }
        }
    }

    IEnumerator CheckInternetLose_Coroutine()
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
            time = 0;
            levelFinished = true;

            services.PostReport(points.ToString(), 1);
            loseMenu.SetActive(true);
        }
    }
}

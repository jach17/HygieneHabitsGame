using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Level2Mini1 : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private TMP_Text txtPuntos;
    [SerializeField]
    private float time;


    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private float spawnWaitUp;
    [SerializeField]
    private float startWaitUp;
    [SerializeField]
    private float spawnWaitDown;
    [SerializeField]
    private float startWaitDown;
    [SerializeField]
    private Vector3 spawnValues;
    [SerializeField]
    private GameObject[] hazards;

    private float points;
    [SerializeField]
    private float maxPoints = 15;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject ServicesGameObject;
    Services services;
    private bool levelFinished;

    [SerializeField]
    private GameObject winMenu;

    [SerializeField]
    private GameObject loseMenu;

    [SerializeField]
    private TMP_Text txtPointsWin;
    // Start is called before the first frame update
    void Start()
    {
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        StartCoroutine(SpawnWavesUp());
        StartCoroutine(SpawnWavesDown());
    }

    // Update is called once per frame
    void Update()
    {
        if (levelFinished)
        {
            return;
        }
        //if (points == maxPoints && !levelFinished)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(CheckInternetWin_Coroutine());
        //    return;
        //}

        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();

            //StartCoroutine(CheckInternetLose_Coroutine());

            if (points >= maxPoints)
            {
               txtPointsWin.text = "Puntaje: " + points.ToString();
                Debug.Log("points>=");
                StartCoroutine(CheckInternetWin_Coroutine());
                winMenu.SetActive(true);
            }
            else
            {
                Debug.Log("lose");
                StartCoroutine(CheckInternetLose_Coroutine());
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

    public void AddPunto()
    {
        points++;
        audioSource.Play();
        txtPuntos.text = "Puntos: " + points.ToString();

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }
    IEnumerator SpawnWavesUp()
    {
        yield return new WaitForSeconds(startWaitUp);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-3.29f, 3.5f), Random.Range(2.87f, 1.07f), 0);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                
             
                yield return new WaitForSeconds(spawnWaitUp);
            }
        }
    }

    IEnumerator SpawnWavesDown()
    {
        yield return new WaitForSeconds(startWaitDown);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-4.78f, 3.86f), Random.Range(-2.44f, -3.84f), 0);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
               
                yield return new WaitForSeconds(spawnWaitDown);
            }
        }
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
            services.PostReport(points.ToString(), 2);
            txtPointsWin.text = "Puntuación: " + points.ToString();
            winMenu.SetActive(true);
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
            levelFinished = true;
            time = 0;
            services.PostReport(points.ToString(), 2);
            loseMenu.SetActive(true);
        }
    }
    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Level2Min2Manager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private TMP_Text txtPuntos;
    [SerializeField]
    private TMP_Text txtPointsWin;
    [SerializeField]
    private float time;


    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private float spawnWaitL;
    [SerializeField]
    private float startWaitL;
    [SerializeField]
    private float spawnWaitR;
    [SerializeField]
    private float startWaitR;
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
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        StartCoroutine(SpawnWavesLeft());
        StartCoroutine(SpawnWavesRight());
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
        if (time <= 0 )
        {
            levelFinished = true;
            StopAllCoroutines();
            //StartCoroutine(CheckInternetLose_Coroutine());
            if (points >= maxPoints)
            {
               
                Debug.Log("points>=");
                StartCoroutine(CheckInternetWin_Coroutine());
                winMenu.SetActive(true);
                txtPointsWin.text = points.ToString();
            }
            else
            {
                Debug.Log("lose");
                StartCoroutine(CheckInternetLose_Coroutine());
                loseMenu.SetActive(true);
            }
            return;
        }
        else{
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
    public void AddPunto()
    {
        points++;
        audioSource.Play();
        txtPuntos.text = "Puntos: " + points.ToString();
    }

    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }

    IEnumerator SpawnWavesLeft()
    {
        yield return new WaitForSeconds(startWaitL);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-5.78f,-2.66f),Random.Range(2.44f, -5.19f),0);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                

                yield return new WaitForSeconds(spawnWaitL);
                

            }
        }
    }

    IEnumerator SpawnWavesRight()
    {
        yield return new WaitForSeconds(startWaitR);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(1.67f, 6.24f), Random.Range(2.94f, -4.54f), 0);
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWaitR);
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
            txtPointsWin.text = points.ToString();
            winMenu.SetActive(true);
            if (PlayerPrefs.GetInt("statusLevel3") == 0)
            {
                PlayerPrefs.SetInt("statusLevel3", 1);
                services.UpdateLevelStatus("3");
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
            levelFinished = true;
            services.PostReport(points.ToString(), 2);
            loseMenu.SetActive(true);
            time = 0;

        }
    }
}

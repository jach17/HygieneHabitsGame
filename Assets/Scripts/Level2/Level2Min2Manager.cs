using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
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
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();

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
        if (points == maxPoints && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            services.PostReport(points.ToString(),3);
            txtPointsWin.text = points.ToString();
            winMenu.SetActive(true);
            if (PlayerPrefs.GetInt("statusLevel3") == 0)
            {
                PlayerPrefs.SetInt("statusLevel3", 1);
                services.UpdateLevelStatus("3");
            }
            return;
        }
        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            services.PostReport(points.ToString(), 3);
            loseMenu.SetActive(true);
            time = 0;
            
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    // Start is called before the first frame update
    void Start()
    {
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();

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
        if (points == maxPoints && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            services.PostReport(points.ToString(), 2);
            return;
        }

        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            time = 0;
            services.PostReport(points.ToString(), 2);
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

    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
}

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

    private bool levelFinished;
    // Start is called before the first frame update
    void Start()
    {
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();

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
            levelFinished = true;
            
            services.PostReport(points.ToString(), 1);
            winMenu.SetActive(true);
            return;
        }

        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            time = 0;
            levelFinished = true;
            
            services.PostReport(points.ToString(), 1);
            winMenu.SetActive(true);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class Level4Min2 : MonoBehaviour
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
    private float startWait;
    [SerializeField]
    private int hazardCount;
    [SerializeField]
    private Vector3 spawnValues;
    [SerializeField]
    private GameObject[] dishes;

    [SerializeField]
    private int dishesToWash;
    [SerializeField]
    private GameObject soap;


    private GameObject currentSoap;
    private ShirtController shirtController;
    private GameObject currentDish;
    private float points;
    private int index = 0;
    private bool levelFinished;
    [SerializeField]
    private float maxPoints = 15;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject ServicesGameObject;
    Services services;

    private bool moveRandom;
    [SerializeField]
    private GameObject winMenu;
    [SerializeField]
    private GameObject loseMenu;
    public bool MoveRandom
    {
        get { return moveRandom; }
        set { moveRandom = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();

        dishes.Shuffle();
        StartCoroutine(SpawnFirtsShirt());
    }

    // Update is called once per frame
    void Update()
    {
        if (levelFinished)
        {
            return;
        }
        MoveRandom = false;
        /*if (points == maxPoints && !levelFinished)
        {
            StopAllCoroutines();
            StartCoroutine(CheckInternetWin_Coroutine());
            return;
        }*/
        /*if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;
            if (points >= maxPoint)
            {
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
        }*/
        if (time <= 0 && !levelFinished)
        {
            StopAllCoroutines();
            levelFinished = true;

            if (points >= maxPoints)
            {
                StartCoroutine(CheckInternetWin_Coroutine());
                winMenu.SetActive(true);
            }
            else
            {
                StartCoroutine(CheckInternetLose_Coroutine());
                loseMenu.SetActive(true);
            }

            //StartCoroutine(CheckInternetLose_Coroutine());
            return;
        }
        else
        {
            time -= Time.deltaTime;
        }

        txtTime.text = time.ToString("F2");





        //DishController dishController = dishes[index].GetComponent<DishController>();

        //Debug.Log(dishController.CanDelete);
        if (shirtController == null)
        {
            return;
        }
        if (shirtController.CanDelete)
        {
            NextShirt();
        }

        /*DishController dishController = dishes[index].GetComponent<DishController>();
        Debug.Log(dishes[index].transform.childCount);
        if (dishController.CanDelete)
        {
            dishesToWash--;
            Debug.Log("AAAAAAAAAAAAAAAAA");
            Destroy(dishes[index]);
            index++;
            puntos++;
            txtPuntos.text = "Puntos: " + puntos.ToString();
            index = Random.Range(0, 9);
            Instantiate(dishes[index], spawnValues, Quaternion.identity);

        }*/
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }

    public void NextShirt()
    {
        dishesToWash--;
        //Debug.Log("AAAAAAAAAAAAAAAAA");
        Destroy(currentDish);
        Destroy(currentSoap);
        index++;
        points++;
        audioSource.Play();
        txtPuntos.text = "Puntos: " + points.ToString();

        /*if (points == maxPoints)
        {
            levelFinished = true;
        }*/
        if (levelFinished)
        {
            return;
        }
        index = Random.Range(0, 6);
        currentDish = Instantiate(dishes[index], spawnValues, Quaternion.identity);
        shirtController = currentDish.GetComponent<ShirtController>();

        //moveRandom = true;
        int spawnSoap = Random.Range(0, 2);
        if (spawnSoap == 0)
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(-8.37f, -4.41f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
        }
        else
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(4.41f, 8.37f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
        }
    }

    IEnumerator SpawnFirtsShirt()
    {
        yield return new WaitForSeconds(startWait);
        index = Random.Range(0, 6);
        currentDish = Instantiate(dishes[index], spawnValues, Quaternion.identity);
        shirtController = currentDish.GetComponent<ShirtController>();
        //moveRandom = true;
        int spawnSoap = Random.Range(0, 2);
        if (spawnSoap == 0)
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(-8.37f, -4.41f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
        }
        else
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(4.41f, 8.37f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
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
            services.PostReport(points.ToString(), 4);
            txtPointsWin.text = "Puntuación: " + points.ToString();
            winMenu.SetActive(true);
            if (PlayerPrefs.GetInt("statusLevel5") == 0)
            {
                PlayerPrefs.SetInt("statusLevel5", 1);
                services.UpdateLevelStatus("5");
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
            services.PostReport(points.ToString(), 4);
            loseMenu.SetActive(true);
            time = 0;
        }
    }
}

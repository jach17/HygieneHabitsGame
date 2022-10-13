using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Min2Manager : MonoBehaviour
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

    private float puntos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWavesLeft());
        StartCoroutine(SpawnWavesRight());
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        txtTime.text = time.ToString("F2");
    }

    public void AddPunto()
    {
        puntos++;
        txtPuntos.text = "Puntos: " + puntos.ToString();
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

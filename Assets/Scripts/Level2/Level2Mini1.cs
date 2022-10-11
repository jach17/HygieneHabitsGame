using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float puntos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWavesUp());
        StartCoroutine(SpawnWavesDown());
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

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        txtTime.text = time.ToString("F2");
    }

    public void AddPoint()
    {
        points++;
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

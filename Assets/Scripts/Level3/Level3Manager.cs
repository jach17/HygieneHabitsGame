using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtTime;
    [SerializeField]
    private TMP_Text txtPuntos;
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
    private DishController dishController;
    private GameObject currentDish;
    private float puntos;
    private int index = 0;

    private bool moveRandom;

    public bool MoveRandom
    {
        get { return moveRandom; }
        set { moveRandom = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        dishes.Shuffle();
        StartCoroutine(SpawnFirtsDish());
    }

    // Update is called once per frame
    void Update()
    {
        MoveRandom = false;
        if (dishesToWash == 0)
        {
            return;
        }
        time -= Time.deltaTime;
        txtTime.text = time.ToString("F2");

        //DishController dishController = dishes[index].GetComponent<DishController>();

        //Debug.Log(dishController.CanDelete);
        if (dishController == null)
        {
            return;
        }
        if (dishController.CanDelete)
        {
            NextDish();
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

    public void NextDish()
    {
        dishesToWash--;
        //Debug.Log("AAAAAAAAAAAAAAAAA");
        Destroy(currentDish);
        Destroy(currentSoap);
        index++;
        puntos++;
        txtPuntos.text = "Puntos: " + puntos.ToString();

        index = Random.Range(0, 9);
        currentDish = Instantiate(dishes[index], spawnValues, Quaternion.identity);
        dishController = currentDish.GetComponent<DishController>();

        //moveRandom = true;
        int spawnSoap = Random.Range(0, 2);
        if (spawnSoap == 0)
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(-8.37f, -4.41f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
        }
        else
        {
            currentSoap = Instantiate(soap, new Vector2(Random.Range(4.41f ,8.37f), Random.Range(2.51f, -3.32f)), Quaternion.identity);
        }
    }

    IEnumerator SpawnFirtsDish()
    {
        yield return new WaitForSeconds(startWait);
        index = Random.Range(0, 9);
        currentDish = Instantiate(dishes[index], spawnValues, Quaternion.identity);
        dishController = currentDish.GetComponent<DishController>();
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
}

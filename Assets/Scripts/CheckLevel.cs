using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLevel : MonoBehaviour
{
    
    [SerializeField]
    private GameObject image;

    public void LevelCheck(int level)
    {
        switch (level)
        {
            case 1:
                if (PlayerPrefs.GetInt("statusLevel1") == 0)
                {
                    image.SetActive(true);
                }
                break;
            case 2:
                if (PlayerPrefs.GetInt("statusLevel2") == 0)
                {
                    image.SetActive(true);
                }
                break;
            case 3:
                if (PlayerPrefs.GetInt("statusLevel3") == 0)
                {
                    image.SetActive(true);
                }
                break;
            case 4:
                if (PlayerPrefs.GetInt("statusLevel4") == 0)
                {
                    image.SetActive(true);
                }
                break;
            case 5:
                if (PlayerPrefs.GetInt("statusLevel5") == 0)
                {
                    image.SetActive(true);
                }
                break;
        }
    }
}

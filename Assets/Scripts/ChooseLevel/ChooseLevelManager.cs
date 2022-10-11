using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevelManager : MonoBehaviour
{
   public void PlayLeve1()
    {
        SceneManager.LoadScene("Level1-1");
    }
    
    public void PlayLeve2()
    {
        SceneManager.LoadScene("Level2-1");
    }
    
    public void PlayLeve3()
    {
        SceneManager.LoadScene("Level2-2");
    }
}

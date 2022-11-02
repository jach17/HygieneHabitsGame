using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject m_MenuPausa, m_Pausa;
    public void PlayHome()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
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
    public void PlayLeve4()
    {
        SceneManager.LoadScene("Level4-1");
    }
    public void PlayLeve5()
    {
        SceneManager.LoadScene("Level5-2");
    }

    public void SignOut()
    {
        PlayerPrefs.DeleteKey("namePlayer");
        PlayerPrefs.DeleteKey("password");
        SceneManager.LoadScene("LoginScene");
    }
}

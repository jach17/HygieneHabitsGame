using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject m_MenuPausa, m_Pausa;
    
   public void Pausa()
    {
        Time.timeScale = 0f;
        m_Pausa.SetActive(false);
        m_MenuPausa.SetActive(true);
    }
    public void Reanudar()
    {
        
        m_Pausa.SetActive(true);
        m_MenuPausa.SetActive(false);
        Time.timeScale = 1f;

    }
    public void PlayLevel5()
    {
        SceneManager.LoadScene("Level5-2");
    }
    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1-1");
    }
    public void PlayLevel21()
    {
        SceneManager.LoadScene("Level2-1");
    }
    public void PlayLevel22()
    {
        SceneManager.LoadScene("Level2-2");
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene("Level3-1");
    }

    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
    public void QuitarIconoPausa()
    {
        m_Pausa.SetActive(false);
    }
}


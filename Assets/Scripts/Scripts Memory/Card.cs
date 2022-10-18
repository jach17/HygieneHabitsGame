using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Card : MonoBehaviour
{
    public int Id { get; set; }
   [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Animator ani;

   void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnMouseDown()
    {
        GameManager.Instancia.ClickFicha(this);
    }

    public void MostrarFrente()
    {
        ani.Play("BackToFront");
    }
    public void MostrarReverso()
    {
        ani.Play("FrontToBack");
    }
    public void SetearImage(Sprite sprite)
    {
        m_SpriteRenderer.sprite = sprite;
    }
    public void PlayLevel5()
    {
        SceneManager.LoadScene("Level5-2");
    }
    public void Return()
    {
        SceneManager.LoadScene("ChooseLevelScene");
    }
}

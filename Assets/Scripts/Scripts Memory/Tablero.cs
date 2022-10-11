using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    [Header("Valores")]
    [SerializeField] int m_AreaDeJuegoX = 4;
    [SerializeField] int m_AreaDeJuegoY = 4;
    [SerializeField] Vector2 m_SeparacionEntreFichas = Vector2.zero;

    [Header("Referencias")]
    [SerializeField] GameObject m_Ficha;
    [SerializeField] Transform AreaDeJuego;
    [SerializeField] private Sprite[] imagenes;


    public void InicializarTablero()
    {
        List<int> IdsFichas = CrearListaDeIdsMezclada(16);

        Vector2 PosicionFichaInicialX = PosInicialFichaX();
        Vector2 PosicionFichaInicialY = PosInicialFichaY();
        int FichasCreadas = 0;
        for (int x = 0; x < m_AreaDeJuegoX; x++)
        {
            for (int y = 0; y < m_AreaDeJuegoY; y++)
            {
                GameObject fichaGO = Instantiate(m_Ficha);
                fichaGO.transform.SetParent(AreaDeJuego);

                Card FichaActual = fichaGO.GetComponent<Card>();
                FichaActual.Id = IdsFichas[FichasCreadas];
                FichaActual.SetearImage(imagenes[FichaActual.Id]);



                fichaGO.transform.localPosition = new Vector3((x * m_SeparacionEntreFichas.x) - PosicionFichaInicialX.x, 0,
                    (y * m_SeparacionEntreFichas.y) - PosicionFichaInicialY.y);

                FichasCreadas++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 PosInicialFichaX()
    {
        float posMaxX = (m_AreaDeJuegoX - 1) * m_SeparacionEntreFichas.x;
        float mitadPos = (posMaxX / 2);
        return new Vector2(mitadPos, 0);
    }
    private Vector2 PosInicialFichaY()
    {
        float posMaxY = (m_AreaDeJuegoY - 1) * m_SeparacionEntreFichas.y;
        float mitadPos = (posMaxY / 2);
        return new Vector2(0, mitadPos);
    }

    private List<int> CrearListaDeIdsMezclada(int cantidad) 
    {

        List<int> idsFichas = new List<int>();
        for (int i = 0; i < cantidad; i++)
        {
            idsFichas.Add(i / 2);
        }
        idsFichas.Shuffle();
        return idsFichas;

    }
}

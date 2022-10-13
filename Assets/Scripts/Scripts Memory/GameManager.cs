using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;
    private Tablero m_tablero;
    private bool m_PuedeSeleccionaFicha = true;
    private Card UltimaFichaSeleccionada = null;
    private int Totaldefichas = 16;
    void Awake()
    {
        Singleton();
        m_tablero = GetComponent<Tablero>();
    }

    
    private void Singleton()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Iicianlizanmos todas las cartas en este metodo
    private void Start()
    {
        m_tablero.InicializarTablero();
    }
    void Update()
    {
        
    }
    //Con este metodo vamos a gestionar la parte logica de el juego
    public void ClickFicha(Card ficha)
    {
        if (!m_PuedeSeleccionaFicha)
        return;
        

        if (!UltimaFichaSeleccionada)
        {
            PrimeraFichaSeleccionada(ficha);
        }
        else
        {
            SegundaFichaSeleccionda(ficha);
        }
    }

    public void PrimeraFichaSeleccionada(Card ficha)
    {
        UltimaFichaSeleccionada = ficha;
        ficha.MostrarFrente();
    }
    public void SegundaFichaSeleccionda(Card ficha) {

        if (ficha == UltimaFichaSeleccionada)
        {
            return;
        }
        ficha.MostrarFrente();
        if (ficha.Id == UltimaFichaSeleccionada.Id)
        {
            ParCorrecto(ficha, UltimaFichaSeleccionada);
        }
        else
        {
            ParIncorrecto(ficha, UltimaFichaSeleccionada);
        }
    }

    private void ParCorrecto(Card ficha, Card ultimaFichaSeleccionada)
    {
        //Destruir ambas fichas
        Destroy(ficha.gameObject, 1.0f);
        Destroy(UltimaFichaSeleccionada.gameObject, 1.0f);
        //Mostrar particulas de acierto
        //Emitir sonido de acierto
        // Resetear la ultima seleccion
        UltimaFichaSeleccionada = null;
        //bloquear seleccion por un momento
        StartCoroutine(BloquearSeleccion(1.5f));
        //Restar las fichas restantes para ganar
        Totaldefichas = Totaldefichas - 2;
        print(Totaldefichas);
        if (Totaldefichas <= 0)
        {
            //Ganamos el juego
        }

    }

    private void ParIncorrecto(Card ficha, Card ultimaFichaSeleccionada)
    {
        StartCoroutine(BloquearAni(1.5f, ficha, ultimaFichaSeleccionada));

      
        UltimaFichaSeleccionada = null;

    }

    IEnumerator BloquearSeleccion(float tiempo)
    {
        m_PuedeSeleccionaFicha = false;
        yield return  new WaitForSeconds(tiempo);
        m_PuedeSeleccionaFicha = true;
    }
    IEnumerator BloquearAni(float tiempo, Card ficha, Card ultimaFichaSeleccionada)
    {
       
        yield return new WaitForSeconds(tiempo);
        ficha.MostrarReverso();
        ultimaFichaSeleccionada.MostrarReverso();

    }

}

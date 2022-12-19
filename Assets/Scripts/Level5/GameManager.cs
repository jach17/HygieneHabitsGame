using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;
    private Tablero m_tablero;
    public bool m_PuedeSeleccionaFicha = true;
    private Card UltimaFichaSeleccionada = null;
    private int Totaldefichas = 16;
    private int intentos = 20;
    public AudioSource error, acierto, principal, victoria, derrota;
    [SerializeField] Particulas particulas;
    [SerializeField] TextMeshProUGUI IntentosRestantesTXT;
    [SerializeField] GameObject Winner, Losser, IntentosRes, m_MenuPausa, m_Pausa;
    [SerializeField] GameObject ServicesGameObject;
    [SerializeField] TMP_Text txtPointsWin;
    private Services services;
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

    //Inicianlizanmos todas las cartas en este metodo
    private void Start()
    {
        m_tablero.InicializarTablero();
        services = ServicesGameObject.GetComponent<Services>();
        string dateStartLevel = DateTime.Now.ToString().Replace("/", "-");
        PlayerPrefs.SetString("dateStartLevel", dateStartLevel);
        PlayerPrefs.Save();

    }
    void Update()
    {
        IntentosRestantesTXT.text = "Intentos restantes: " + intentos;
    }

    /*private void OnApplicationQuit()
    {

        PlayerPrefs.DeleteKey("activeSesion");
        PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
        PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
    }*/
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {

            PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
            PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));

        }

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
    public void SegundaFichaSeleccionda(Card ficha)
    {

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

    private async void ParCorrecto(Card ficha, Card ultimaFichaSeleccionada)
    {
        Totaldefichas = Totaldefichas - 2;
        intentos = intentos - 1;
        if (intentos <= 0)
        {
            if (Totaldefichas <= 0)
            {
                //StartCoroutine(MostrarPantalladeVictoria());
                //StartCoroutine(CheckInternetWin_Coroutine());
                await CheckInternetWin_Async();
                m_PuedeSeleccionaFicha = false;

            }
            else
            {
                //StartCoroutine(MostrarPantalladeDerrota());
                //StartCoroutine(CheckInternetLose_Coroutine());
                await CheckInternetLose_Async();
                m_PuedeSeleccionaFicha = false;
            }
        }
        if (Totaldefichas <= 0)
        {
            //StartCoroutine(MostrarPantalladeVictoria());
            //StartCoroutine(CheckInternetWin_Coroutine());
            await CheckInternetWin_Async();
            m_PuedeSeleccionaFicha = false;

        }



        //Destruir ambas fichas
        Destroy(ficha.gameObject, 2.0f);
        Destroy(UltimaFichaSeleccionada.gameObject, 2.0f);
        //Mostrar particulas de acierto
        StartCoroutine(CrearParticulas(ficha, ultimaFichaSeleccionada));
        //Emitir sonido de acierto
        StartCoroutine(SoundAcierto());
        // Resetear la ultima seleccion
        UltimaFichaSeleccionada = null;
        //bloquear seleccion por un momento
        StartCoroutine(BloquearSeleccion(2f));



    }

    private void ParIncorrecto(Card ficha, Card ultimaFichaSeleccionada)
    {
        intentos = intentos - 1;
        if (intentos <= 0)
        {
            StartCoroutine(MostrarPantalladeDerrota());
            m_PuedeSeleccionaFicha = false;

        }

        UltimaFichaSeleccionada = null;
        StartCoroutine(BloquearAni(1.5f, ficha, ultimaFichaSeleccionada));
        StartCoroutine(SoundError());
        StartCoroutine(BloquearSeleccion(2f));

    }

    IEnumerator BloquearSeleccion(float tiempo)
    {
        m_PuedeSeleccionaFicha = false;
        yield return new WaitForSeconds(tiempo);
        m_PuedeSeleccionaFicha = true;
    }
    IEnumerator SoundError()
    {
        yield return new WaitForSeconds(1.5f);
        error.Play();

    }
    IEnumerator SoundAcierto()
    {
        yield return new WaitForSeconds(1f);
        acierto.Play();

    }
    IEnumerator BloquearAni(float tiempo, Card ficha, Card ultimaFichaSeleccionada)
    {

        yield return new WaitForSeconds(tiempo);
        ficha.MostrarReverso();
        ultimaFichaSeleccionada.MostrarReverso();

    }
    IEnumerator CrearParticulas(Card ficha, Card ultimaFichaSeleccionada)
    {

        yield return new WaitForSeconds(1f);
        particulas.EmitirParitculas(ficha.transform);
        particulas.EmitirParitculas(ultimaFichaSeleccionada.transform);


    }
    IEnumerator MostrarPantalladeDerrota()
    {
        IntentosRes.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Losser.SetActive(true);
        derrota.Play();
        m_Pausa.SetActive(false);

    }
    IEnumerator MostrarPantalladeVictoria()
    {
        IntentosRes.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Winner.SetActive(true);
        victoria.Play();
        m_Pausa.SetActive(false);

    }

    public void Pausa()
    {
        m_PuedeSeleccionaFicha = false;
        m_Pausa.SetActive(false);
        m_MenuPausa.SetActive(true);
    }
    public void Reanudar()
    {
        StartCoroutine(BloquearSeleccion(1f));
        m_Pausa.SetActive(true);
        m_MenuPausa.SetActive(false);


    }

    IEnumerator CheckInternetWin_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            StartCoroutine(MostrarPantalladeVictoria());
            services.PostReport(intentos.ToString(), 1);
            txtPointsWin.text = "Puntuación: " + intentos.ToString();

        }
    }

    IEnumerator CheckInternetLose_Coroutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            StartCoroutine(MostrarPantalladeDerrota());
            services.PostReport(intentos.ToString(), 1);

        }
    }

    async Task CheckInternetWin_Async()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield();
        }

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            StartCoroutine(MostrarPantalladeVictoria());
            //services.PostReport(intentos.ToString(), 1);
            await services.PostReport_Aync(intentos.ToString(), 1);
            txtPointsWin.text = "Puntuación: " + intentos.ToString();

        }
    }

    async Task CheckInternetLose_Async()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield();
        }

        if (request.error != null)
        {
            Debug.Log("Error de conexion ");
            LevelDirection.Level = null;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            StartCoroutine(MostrarPantalladeDerrota());
            //services.PostReport(intentos.ToString(), 1);
            await services.PostReport_Aync(intentos.ToString(), 1);

        }
    }
}

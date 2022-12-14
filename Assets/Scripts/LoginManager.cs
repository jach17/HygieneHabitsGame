using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Net;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private GameObject iniciarSesionForm;
    [SerializeField]
    private GameObject registrarseForm;
    [SerializeField]
    private GameObject mainForm;
    [SerializeField]
    private GameObject loadingGif;
    [SerializeField]
    private GameObject loadingGif2;
    /*[SerializeField]
    private InputField txtNombre;
    [SerializeField]
    private InputField txtPassword;
    [SerializeField]
    private InputField txtEdad;
    [SerializeField]
    private InputField txtIdTutor;
    [SerializeField]
    private InputField txt;*/
    //JSONNode info;
    private bool formActive,reg;
    private string user, password,edad,token,idtutor;
    [SerializeField]
    private GameObject servicesGameObject;
    private Services services;
    //[SerializeField]
    //private Button btnPlay;
    // Start is called before the first frame update
    //Comment to test git
    void Start()
    {
        services = servicesGameObject.GetComponent<Services>();
        
    }

    // Update is called once per frame
    //Comment to test AlexBranch
    void Update()
    {

    }
    private void OnApplicationQuit()
    {
        if (LevelDirection.Level != null)
        {
            PlayerPrefs.DeleteKey("activeSesion");
            PlayerPrefs.SetString("dateEnd", DateTime.Now.ToString().Replace("/", "-"));
            PlayerPrefs.SetInt("oldSesion", PlayerPrefs.GetInt("idSesion"));
        }

    }
    //Metodo que se ejecuta cuando se pulsa el boton de enviar en el formulario de registro
    public void Send()
    {
        Debug.Log("Send");
        //PostPlayer();
        GetUsers();
        //GetUserById("9");
        //PostAuth();
    }

    //Metodo para obtener la informacion del usuario guardada en el telefono, si ya existe la cuenta se salta la escena de login
    public async void GetAccount()
    {
        user = PlayerPrefs.GetString("namePlayer");
        password = PlayerPrefs.GetString("password");
        //PostAuth(user, password,true);
        //StartCoroutine(CheckInternetAuth_Coroutine(true));
        await CheckInternetAuth_Async(true);
    }

    //Metodo que se llama cuando se deja de escribir en el txt de usuario
    public void ReadUser(string s)
    {
        user = s;
    }

    //Metodo que se llama cuando se deja de escribir en el txt de la contrase?a
    public void ReadPassword(string s)
    {
        password = s;
    }

    public void ReadEdad(string s)
    {
 

        edad = s;
    }

    public void ReadToken(string s)
    {


        token = s;
    }

    public void ReadIdTutor(string s)
    {

        idtutor = s;
    }

    

    public void ShowMainForm()
    {
        if (formActive)
        {
            iniciarSesionForm.SetActive(false);
            mainForm.SetActive(true);
            
        }
        else
        {
            registrarseForm.SetActive(false);
            mainForm.SetActive(true);
            
        }
    }

    public void ShowIniciarSesionForm()
    {
        iniciarSesionForm.SetActive(true);
        mainForm.SetActive(false);
        formActive = true;
    }

    public void ShowRegistrarseForm()
    {
        registrarseForm.SetActive(true);
        mainForm.SetActive(false);
        formActive = false;
    }
    public async void AuthUser()
    {
        if (user != "" && password != "")
        {
           
        }
        //PostAuth(user, password,false);
        loadingGif.SetActive(true);
        //StartCoroutine(CheckInternetAuth_Coroutine(false));
        await CheckInternetAuth_Async(false);
        
    }
    public async void RegistrarUsuario()
    {
        /*if (user != "" && password != "" && edad != "" && idtutor != "" && token != "")
        {
            
        }*/

        //PostPlayer(user, password, edad, idtutor, token);
        loadingGif2.SetActive(true);
        //StartCoroutine(CheckInternetPostPlayer_Coroutine());
        await CheckInternetPostPlayer_Async();
    }

    public void GetUsers() => StartCoroutine(GetUsers_Coroutine());
    public void GetUserById(string id) => StartCoroutine(GetUserById_Coroutine(id));
    public void PostPlayer(string nombre,string password, string edad, string id, string token) => StartCoroutine(PostPlayer_Coroutine(nombre,password,edad,id,token));
    public void PostAuth(String user, String password, bool autoLogin) => StartCoroutine(PostAuthPlayer_Coroutine(user,password, autoLogin));
    
    IEnumerator PostAuthPlayer_Coroutine(String user,String password, bool autoLogin)
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/auth/player";
        int idPlayer = 0;
        AuthUser usr = new AuthUser(user,password);
        var json = JsonConvert.SerializeObject(usr);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        //Debug.Log(json);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                //String response = webRequest.downloadHandler.text;
                //JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                reg = info["message"]["response"][0]["isRegistred"];
                idPlayer = info["message"]["response"][0]["idPlayer"];
                Debug.Log(info);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        if (isRegistred())
        {
            if (!autoLogin)
            {
                PlayerPrefs.SetString("namePlayer", user);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetInt("idPlayer",idPlayer);
                PlayerPrefs.Save();
                if (services == null)
                {
                    Debug.Log("NULL AAAAAAAAAAAAA");
                }
                else
                {
                    services.GetUserById();
                }
               
            }
            SceneManager.LoadScene("ChooseLevelScene");
        }
        else
        {
            Debug.Log("NO ESTA REGISTRADOO!");
        }
        yield break;
    }

    IEnumerator PostPlayer_Coroutine(String nombre, String password, String edad, string id, String token)
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/add/player";
        int idTutor = Convert.ToInt32(id);
        int idPlayer = 0;
        Player player = new Player(nombre,password,edad,/*idTutor,*/token);
        var json = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        Debug.Log(json);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                reg = info["message"]["response"][0]["inserted"];
                idPlayer = info["message"]["response"][0]["insertedId"];
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        if (reg)
        {
            PlayerPrefs.SetString("namePlayer", nombre);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetInt("idPlayer", idPlayer);
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("ERROR AL REGISTRAR USUARIO");
        }

        yield break;
    }

    IEnumerator GetUsers_Coroutine()
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/list/players";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
            request.Dispose();
        }
    }

    IEnumerator GetUserById_Coroutine(String id)
    {
        JSONNode itemsData;
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/list/player/" + id;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                itemsData = JSON.Parse(request.downloadHandler.text);
                Debug.Log("Nombre: " + itemsData["message"]["response"][0]["namePlayer"]);
                Debug.Log("Edad: " + itemsData["message"]["response"][0]["agePlayer"]);
                Debug.Log("Tutor: " + itemsData["message"]["response"][0]["idTutorOwner"]);
            }
            request.Dispose();
        }
    }

    IEnumerator CheckInternetAuth_Coroutine(bool autologin)
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
            PostAuth(user, password, autologin);
        }
        request.Dispose();
    }

    IEnumerator CheckInternetPostPlayer_Coroutine()
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
            PostPlayer(user, password, edad, idtutor, token);
        }
        request.Dispose();
    }

    async Task CheckInternetAuth_Async(bool autologin)
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
            //PostAuth(user, password, autologin);
            await PostAuthPlayer_Async(user, password, autologin);
        }
        request.Dispose();
    }

    async Task CheckInternetPostPlayer_Async()
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
            //PostPlayer(user, password, edad, idtutor, token);
            await PostPlayer_Async(user, password,edad,/* idtutor,*/ token);
        }
        request.Dispose();
    }


    private bool isRegistred()
    {

        if (reg)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    async Task PostAuthPlayer_Async(string user, string password, bool autoLogin)
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/auth/player";
        int idPlayer = 0;
        AuthUser usr = new AuthUser(user, password);
        var json = JsonConvert.SerializeObject(usr);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        //Debug.Log(json);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                //String response = webRequest.downloadHandler.text;
                //JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                reg = info["message"]["response"][0]["isRegistred"];
                idPlayer = info["message"]["response"][0]["idPlayer"];
                Debug.Log(info);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        if (isRegistred())
        {
            if (!autoLogin)
            {
                
                PlayerPrefs.SetString("namePlayer", user);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetInt("idPlayer", idPlayer);
                PlayerPrefs.Save();
                if (services == null)
                {
                    Debug.Log("NULL AAAAAAAAAAAAA");
                }
                else
                {
                    services.GetUserById();
                }
            }
            SceneManager.LoadScene("ChooseLevelScene");
        }
        else
        {
            Debug.Log("NO ESTA REGISTRADOO!");
        }
    }

    async Task PostPlayer_Async(string nombre, string password, string edad, /*string id,*/ string token)
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/add/player";
        //int idTutor = Convert.ToInt32(id);
        int idPlayer = 0;
        Player player = new Player(nombre, password, edad, /*idTutor,*/ token);
        var json = JsonConvert.SerializeObject(player);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        Debug.Log(json);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                reg = info["message"]["response"][0]["inserted"];
                idPlayer = info["message"]["response"][0]["insertedId"];
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        if (reg)
        {
            PlayerPrefs.SetString("namePlayer", nombre);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetInt("idPlayer", idPlayer);
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("ERROR AL REGISTRAR USUARIO");
        }
    }
}

[Serializable]
public class AuthUser
{
    public String namePlayer;
    public String passwordPlayer;

    public AuthUser(String namePlayer, String passwordPlayer)
    {
        this.namePlayer = namePlayer;
        this.passwordPlayer = passwordPlayer;
    }

}

[Serializable]
public class Player
{
    public String namePlayer;
    public String passwordPlayer;
    public String agePlayer;
    public int idTutorOwner;
    public String authTokenTutor;

    public Player(String namePlayer, String passwordPlayer, string agePlayer, /*int idTutorOwner,*/ string authTokenTutor)
    {
        this.namePlayer = namePlayer;
        this.passwordPlayer = passwordPlayer;
        this.agePlayer = agePlayer;
        //this.idTutorOwner = idTutorOwner;
        this.authTokenTutor = authTokenTutor;
    }

}
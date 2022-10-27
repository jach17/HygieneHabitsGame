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

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainForm;
    [SerializeField]
    private GameObject signUpForm;

    private string user, password;
    //[SerializeField]
    //private Button btnPlay;
    // Start is called before the first frame update
    //Comment to test git
    void Start()
    {

    }

    // Update is called once per frame
    //Comment to test AlexBranch
    void Update()
    {

    }

    //Metodo que se llama cuando se pulsa el boton de play
    public void Play()
    {
        Debug.Log("Play");
    }

    //Metodo que se llama cuando se pulsa el boton de Crear cuenta
    public void SignUp()
    {
        Debug.Log("SignUp");

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
    public void GetAccount()
    {
        Debug.Log("GetAccount");

    }

    //Metodo que se llama cuando se deja de escribir en el txt de usuario
    public void ReadUser(string s)
    {
        user = s;
    }

    //Metodo que se llama cuando se deja de escribir en el txt de la contraseña
    public void ReadPassword(string s)
    {
        password = s;
    }

    public void GetUsers() => StartCoroutine(GetUsers_Coroutine());
    public void GetUserById(String id) => StartCoroutine(GetUserById_Coroutine(id));
    public void PostPlayer() => StartCoroutine(PostPlayer_Coroutine());
    public void PostAuth() => StartCoroutine(PostAuthPlayer_Coroutine());
    
    IEnumerator PostAuthPlayer_Coroutine()
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/auth/player";

        AuthUser usr = new AuthUser("Player123", "123");
        var json = JsonConvert.SerializeObject(usr);

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
                Debug.Log(data);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }

        yield break;
    }

    IEnumerator PostPlayer_Coroutine()
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/add/player";

        Player player = new Player("AAAAAA", "RRRRRRR", "12", 1, "123abc");
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
                Debug.Log(data);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
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

    public Player(String namePlayer, String passwordPlayer, string agePlayer, int idTutorOwner, string authTokenTutor)
    {
        this.namePlayer = namePlayer;
        this.passwordPlayer = passwordPlayer;
        this.agePlayer = agePlayer;
        this.idTutorOwner = idTutorOwner;
        this.authTokenTutor = authTokenTutor;
    }

}
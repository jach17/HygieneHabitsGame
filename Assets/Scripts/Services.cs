using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Services : MonoBehaviour
{
    private bool reg;

    public void PostReport(string currentScoreLevel, int idLevelPlayed) => StartCoroutine(PostReport_Coroutine(currentScoreLevel, idLevelPlayed));
    public void PostSesion(string dateStart, string dateEnd) => StartCoroutine(PostSesion_Coroutine(dateStart, dateEnd));
    public void UpdateSesion() => StartCoroutine(UpdateSesion_Coroutine());

    public void PostAuth(string user, String password) => StartCoroutine(PostAuthPlayer_Coroutine(user, password));

    IEnumerator PostAuthPlayer_Coroutine(String user, String password)
    {
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/auth/player";

        AuthUser usr = new AuthUser(user, password);
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
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        if (isRegistred())
        {

            SceneManager.LoadScene("ChooseLevelScene");
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
        yield break;
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


    IEnumerator PostReport_Coroutine(string currentScoreLevel, int idLevelPlayed)
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/add/report";

        string dateEndLevel = DateTime.Now.ToString().Replace("/", "-");
        Report report = new Report(PlayerPrefs.GetString("dateStartLevel"), dateEndLevel, PlayerPrefs.GetInt("idSesion"), currentScoreLevel, idLevelPlayed);
        var json = JsonConvert.SerializeObject(report);

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
                Debug.Log(info);
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }

        yield break;
    }

    IEnumerator PostSesion_Coroutine(string dateStart, string dateEnd)
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/add/sesion";

        Sesion sesion = new Sesion(dateStart, dateEnd, PlayerPrefs.GetInt("idPlayer"));
        var json = JsonConvert.SerializeObject(sesion);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
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
                int idSesion = info["message"]["response"][0]["insertedId"];
                PlayerPrefs.SetInt("idSesion", idSesion);
                PlayerPrefs.Save();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        if (PlayerPrefs.GetString("dateEnd") != "")
        {
            UpdateSesion();
        }
        yield break;
    }

    IEnumerator UpdateSesion_Coroutine()
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/update/sesion/" + PlayerPrefs.GetInt("oldSesion").ToString();
        Debug.Log(PlayerPrefs.GetInt("idSesion"));
        Debug.Log(PlayerPrefs.GetInt("oldSesion"));

        dateEndc date = new dateEndc(PlayerPrefs.GetString("dateEnd"));
        var json = JsonConvert.SerializeObject(date);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "UPDATE");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
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
                Debug.Log(info);

                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        PlayerPrefs.DeleteKey("dateEnd");
        yield break;
    }
}

[Serializable]
public class Report
{
    public string dateStartLevel;
    public string dateEndLevel;
    public int idSesionOwner;
    public string currentScoreLevel;
    public int idLevelPlayed;

    public Report(string dateStartLevel, string dateEndLevel, int idSesionOwner, string currentScoreLevel, int idLevelPlayed)
    {
        this.dateStartLevel = dateStartLevel;
        this.dateEndLevel = dateEndLevel;
        this.idSesionOwner = idSesionOwner;
        this.currentScoreLevel = currentScoreLevel;
        this.idLevelPlayed = idLevelPlayed;
    }
}

[Serializable]
public class Sesion
{
    public string dateStart;
    public string dateEnd;
    public int idPlayerOwner;

    public Sesion(string dateStart, string dateEnd, int idPlayerOwner)
    {
        this.dateStart = dateStart;
        this.dateEnd = dateEnd;
        this.idPlayerOwner = idPlayerOwner;
    }
}

[Serializable]
public class dateEndc
{
    public string dateEnd;

    public dateEndc(string dateEnd)
    {
        this.dateEnd = dateEnd;
    }
}
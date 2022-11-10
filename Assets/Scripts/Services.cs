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

    public void GetUserById() => StartCoroutine(GetUserById_Coroutine());
    public void PostReport(string currentScoreLevel, int idLevelPlayed) => StartCoroutine(PostReport_Coroutine(currentScoreLevel, idLevelPlayed));
    public void PostSesion(string dateStart, string dateEnd) => StartCoroutine(PostSesion_Coroutine(dateStart, dateEnd));
    public void UpdateSesion() => StartCoroutine(UpdateSesion_Coroutine());

    public void PostAuth(string user, String password) => StartCoroutine(PostAuthPlayer_Coroutine(user, password));
    public void UpdateLevelStatus(string level) => StartCoroutine(UpdateLevelStatus_Coroutine(level));

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
                Debug.Log(info);
                reg = info["message"]["response"][0]["isRegistred"];
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        if (isRegistred())
        {
            GetUserById();
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
                //Debug.Log(info);

                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        PlayerPrefs.DeleteKey("dateEnd");
        yield break;
    }

    IEnumerator UpdateLevelStatus_Coroutine(string level)
    {
        string url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/enable/level/" + level + "/player/" + PlayerPrefs.GetInt("idPlayer");

        dateEndc date = new dateEndc(PlayerPrefs.GetString("dateEnd"));
        var json = JsonConvert.SerializeObject(date);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "UPDATE");
        //webRequest.SetRequestHeader("Content-Type", "application/json");
        //byte[] rawData = Encoding.UTF8.GetBytes(json);
        //webRequest.uploadHandler = new UploadHandlerRaw(rawData);
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

    IEnumerator GetUserById_Coroutine()
    {
        JSONNode itemsData;
        String url = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits/list/player/" + PlayerPrefs.GetInt("idPlayer").ToString();
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                itemsData = JSON.Parse(request.downloadHandler.text);
                PlayerPrefs.SetInt("statusLevel1", itemsData["message"]["response"][0]["statusLevel1"]);
                PlayerPrefs.SetInt("statusLevel2", itemsData["message"]["response"][0]["statusLevel2"]);
                PlayerPrefs.SetInt("statusLevel3", itemsData["message"]["response"][0]["statusLevel3"]);
                PlayerPrefs.SetInt("statusLevel4", itemsData["message"]["response"][0]["statusLevel4"]);
                PlayerPrefs.SetInt("statusLevel5", itemsData["message"]["response"][0]["statusLevel5"]);
                Debug.Log(itemsData);
            }
        }
        SceneManager.LoadScene("ChooseLevelScene");
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
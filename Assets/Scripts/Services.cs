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
using System.Net;
using System.Threading.Tasks;


public class Services : MonoBehaviour
{
    private string urlApi = "https://hygienehabitsback-production.up.railway.app/api/hygienehabits";
    private bool reg;
    private bool getStatusUserById;
    private bool getStatusAuth;
    private bool isConnected;
    public void GetUserById() => StartCoroutine(GetUserById_Coroutine());

    /*public bool GetUserById()
    {
        StartCoroutine(GetUserById_Coroutine(returnValue =>
        {
            getStatusUserById = returnValue;
        }));
        return getStatusUserById;
    }*/
    public void PostReport(string currentScoreLevel, int idLevelPlayed) => StartCoroutine(PostReport_Coroutine(currentScoreLevel, idLevelPlayed));
    public void PostSesion(string dateStart, string dateEnd) => StartCoroutine(PostSesion_Coroutine(dateStart, dateEnd));
    public void UpdateSesion() => StartCoroutine(UpdateSesion_Coroutine());
    public void GetReports() => StartCoroutine(GetReports_Coroutine());
    public void GetPlayers() => StartCoroutine(GetPlayers_Coroutine());
    public void GetSesions() => StartCoroutine(GetSesions_Coroutine());
    /*public bool PostAuth(string user, string password)
    {
        StartCoroutine(PostAuthPlayer_Coroutine(user, password, returnValue =>
        {
            getStatusAuth = returnValue;
        }));
        return getStatusAuth;
    }*/
    public void PostAuth(string user, string password) => StartCoroutine(PostAuthPlayer_Coroutine(user, password));

    public void UpdateLevelStatus(string level) => StartCoroutine(UpdateLevelStatus_Coroutine(level));
    /*public bool CheckConnection()
    {
        StartCoroutine(CheckInternet_Coroutine((isConnected) =>
        {
            if (isConnected)
            {
                isConnected = true;
            }
            else
            {
                isConnected = false;
            }
        }));

        return isConnected;
    }*/

    IEnumerator PostAuthPlayer_Coroutine(string user, string password)
    {
        string url = urlApi + "/auth/player";

        AuthUser usr = new AuthUser(user, password);
        var json = JsonConvert.SerializeObject(usr);
        int idPlayer = 0;
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
                Debug.Log("IN PROGRESS");
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                //String response = webRequest.downloadHandler.text;
                //JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                //Debug.Log(info);
                reg = info["message"]["response"][0]["isRegistred"];
                idPlayer = info["message"]["response"][0]["idPlayer"];
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                Debug.Log("ERROR");
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
        if (isRegistred())
        {
            PlayerPrefs.SetString("namePlayer", user);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetInt("idPlayer", idPlayer);
            PlayerPrefs.Save();
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
        string url = urlApi + "/add/report";

        string dateEndLevel = DateTime.Now.ToString().Replace("/", "-");
        Report report = new Report(PlayerPrefs.GetString("dateStartLevel"), dateEndLevel, PlayerPrefs.GetInt("idSesion"), currentScoreLevel, idLevelPlayed);
        //Report report = new Report(PlayerPrefs.GetString("dateStartLevel"), dateEndLevel, 89, currentScoreLevel, idLevelPlayed);
        //Report report = new Report("08-12-2022 11:00", "08-12-2022 12:00", 119, "3", 3);
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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log(info);
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
        yield break;
    }

    IEnumerator PostSesion_Coroutine(string dateStart, string dateEnd)
    {
        string url = urlApi + "/add/sesion";

        Sesion sesion = new Sesion(dateStart, dateEnd, PlayerPrefs.GetInt("idPlayer"));
        //Sesion sesion = new Sesion("09-12-2022 7:00", "09 - 12 - 2022 8:00",35);
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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                int idSesion = info["message"]["response"][0]["insertedId"];
                PlayerPrefs.SetInt("idSesion", idSesion);
                PlayerPrefs.Save();
                Debug.Log(info); 
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text); 
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
        if (PlayerPrefs.GetString("dateEnd") != "")
        {
            UpdateSesion();
        }
        yield break;
    }

    IEnumerator UpdateSesion_Coroutine()
    {
        string url = urlApi + "/update/sesion/" + PlayerPrefs.GetInt("oldSesion").ToString();

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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                //String response = webRequest.downloadHandler.text;
                //JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log(info);
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        PlayerPrefs.DeleteKey("dateEnd");
        yield break;
    }

    IEnumerator UpdateLevelStatus_Coroutine(string level)
    {
        string url = urlApi + "/enable/level/" + level + "/player/" + PlayerPrefs.GetInt("idPlayer");

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
        webRequest.Dispose();
        PlayerPrefs.DeleteKey("dateEnd");
        yield break;
    }

    IEnumerator GetUserById_Coroutine()
    {
        JSONNode itemsData;
        string url = urlApi + "/list/player/" + PlayerPrefs.GetInt("idPlayer").ToString();
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
                SceneManager.LoadScene("ChooseLevelScene");
            }
            request.Dispose();
        }

        //SceneManager.LoadScene("ChooseLevelScene");
    }
    IEnumerator GetPlayers_Coroutine()
    {
        JSONNode itemsData;
        string url = urlApi + "/list/players";
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
                Debug.Log(itemsData);
            }
            request.Dispose();
        }

    }
    IEnumerator GetSesions_Coroutine()
    {
        JSONNode itemsData;
        string url = urlApi + "/list/sesions";
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
                Debug.Log(itemsData);
            }
            request.Dispose();
        }

    }
    IEnumerator GetReports_Coroutine()
    {
        JSONNode itemsData;
        string url = urlApi + "/list/reports";
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
                Debug.Log(itemsData);
            }
            request.Dispose();
        }
    }
    //ASYNC 
    public async Task PostAuthPlayer_Async(string user, string password)
    {
        string url = urlApi + "/auth/player";
        AuthUser usr = new AuthUser(user, password);
        var json = JsonConvert.SerializeObject(usr);
        int idPlayer = 0;
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
                Debug.Log("IN PROGRESS");
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                //String response = webRequest.downloadHandler.text;
                //JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                //Debug.Log(info);
                reg = info["message"]["response"][0]["isRegistred"];
                idPlayer = info["message"]["response"][0]["idPlayer"];
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                Debug.Log("ERROR");
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
        if (isRegistred())
        {
            PlayerPrefs.SetString("namePlayer", user);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetInt("idPlayer", idPlayer);
            PlayerPrefs.Save();
            await GetUserById_Async();
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    public async Task GetUserById_Async()
    {
        JSONNode itemsData;
        string url = urlApi + "/list/player/" + PlayerPrefs.GetInt("idPlayer").ToString();
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            itemsData = JSON.Parse(webRequest.downloadHandler.text);
            PlayerPrefs.SetInt("statusLevel1", itemsData["message"]["response"][0]["statusLevel1"]);
            PlayerPrefs.SetInt("statusLevel2", itemsData["message"]["response"][0]["statusLevel2"]);
            PlayerPrefs.SetInt("statusLevel3", itemsData["message"]["response"][0]["statusLevel3"]);
            PlayerPrefs.SetInt("statusLevel4", itemsData["message"]["response"][0]["statusLevel4"]);
            PlayerPrefs.SetInt("statusLevel5", itemsData["message"]["response"][0]["statusLevel5"]);
            PlayerPrefs.DeleteKey("activeSesion");
            PlayerPrefs.Save();
            Debug.Log(itemsData);
            SceneManager.LoadScene("ChooseLevelScene");
        }
        webRequest.Dispose();
    }
    public async Task PostReport_Aync(string currentScoreLevel, int idLevelPlayed)
    {
        string url = urlApi + "/add/report";

        string dateEndLevel = DateTime.Now.ToString().Replace("/", "-");
        Report report = new Report(PlayerPrefs.GetString("dateStartLevel"), dateEndLevel, PlayerPrefs.GetInt("idSesion"), currentScoreLevel, idLevelPlayed);
        Debug.Log(PlayerPrefs.GetString("dateStartLevel"));
        Debug.Log(dateEndLevel);
        var json = JsonConvert.SerializeObject(report);

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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log(info);
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
    }
    public async Task PostSesion_Async(string dateStart, string dateEnd)
    {
        string url = urlApi + "/add/sesion";
        Debug.Log("postSesion");
        Sesion sesion = new Sesion(dateStart, dateEnd, PlayerPrefs.GetInt("idPlayer"));
        var json = JsonConvert.SerializeObject(sesion);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                String response = webRequest.downloadHandler.text;
                JObject data = JObject.Parse(response);
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                int idSesion = info["message"]["response"][0]["insertedId"];
                PlayerPrefs.SetInt("idSesion", idSesion);
                PlayerPrefs.SetString("activeSesion", "true");
                PlayerPrefs.Save();
                Debug.Log(info);
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                webRequest.Dispose();
                break;
        }
        webRequest.Dispose();
    }
    public async Task<bool> UpdateSesion_Async()
    {
        string url = urlApi + "/update/sesion/" + PlayerPrefs.GetInt("oldSesion").ToString();
        dateEndc date = new dateEndc(PlayerPrefs.GetString("dateEnd"));
        var json = JsonConvert.SerializeObject(date);
        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "UPDATE");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(json);
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
                webRequest.Dispose();
                break;
            case UnityWebRequest.Result.Success:
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log(info);
                webRequest.Dispose();
                PlayerPrefs.DeleteKey("dateEnd");
                return true;
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                return false;
                break;
        }
        webRequest.Dispose();
        return true;
    }

    public async Task UpdateLevelStatus_Async(string level)
    {
        string url = urlApi + "/enable/level/" + level + "/player/" + PlayerPrefs.GetInt("idPlayer");

        dateEndc date = new dateEndc(PlayerPrefs.GetString("dateEnd"));
        var json = JsonConvert.SerializeObject(date);

        using UnityWebRequest webRequest = UnityWebRequest.Post(url, "UPDATE");
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
                JSONNode info = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log(info);

                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.downloadHandler.text);
                break;
        }
        webRequest.Dispose();
        PlayerPrefs.DeleteKey("dateEnd");
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
}
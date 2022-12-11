using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSet : MonoBehaviour
{
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
}

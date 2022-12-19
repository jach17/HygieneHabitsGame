using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPruebas : MonoBehaviour
{
    
    private Services services;
    // Start is called before the first frame update
    void Start()
    {
        services = GetComponent<Services>();
        //services.GetReports();
        //services.PostSesion("aaa", "aaa");
        //services.PostReport("3", 3);
        //services.GetPlayers();
        services.GetSesions();
        //string dateEndLevel = DateTime.Now.ToString().Replace("/", "-");
        //Debug.Log(dateEndLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

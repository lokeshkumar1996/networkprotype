using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public static GameStarter Instance { set; get;}
    public GameObject mainMenu;
    public GameObject servermenu; 
    public GameObject connectmenu;

    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public InputField nameinput;



   
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        connectmenu.SetActive(false);
        servermenu.SetActive(false);
        mainMenu.SetActive(true);

        
        DontDestroyOnLoad(gameObject);
    }

    public void MenuConnectButton()
    {
        Debug.Log("connect");
    }

    public void MenuHostButton()
    {
        try
        {
           
            Server s =Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();
            Client c =Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameinput.text;
            c.isHost = true;
            if(c.clientName == "")
                c.clientName = "client";
            c.ConnectToServer("127.0.0.1",6321);
        }
        catch (Exception e)
        {            
           Debug.Log(e.Message);              
           
        }
    }

    public void ConnecToServerButton()
    {
        string hostAddress = GameObject.Find("InpuIP").GetComponent<InputField>().text;
        
        if(hostAddress == "")
            hostAddress = "127.0.0.1";

        try
        {
            Client c =Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameinput.text;
            if(c.clientName == "")
                c.clientName = "client";
            c.ConnectToServer(hostAddress,6321);
            connectmenu.SetActive(false);
            
        }
        catch (Exception e)
        {            
           Debug.Log(e.Message);              
           
        }


    }

    public void BackButton()
    {
        connectmenu.SetActive(false);
        servermenu.SetActive(false);
        mainMenu.SetActive(true);
        
        Server s =FindObjectOfType<Server>();
        if(s != null)
        Destroy(s.gameObject);

        Client c =FindObjectOfType<Client>();
        if(c != null)
        Destroy(c.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }
}


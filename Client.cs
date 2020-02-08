using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;

public class Client : MonoBehaviour
{
    public string clientName;
    public bool isHost;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
 //game variables
    public bool Aa= false;
    public bool Ad= false;
    public bool Ba= false;
    public bool Bd= false;
    public bool Ak= false; // a input done
    public bool Bk= false; // b input done
    public int As;  // score of a
    public int Bs;
    
//
   
    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //socketReady =false;
    }

    public bool ConnectToServer(string host, int port)
    {
        if(socketReady)
            return false;

        try
        {
           socket = new TcpClient(host,port);
           stream = socket.GetStream();
           writer = new StreamWriter(stream);
           reader = new StreamReader(stream);
           socketReady =true;
        }
        catch (Exception e)
        {
            
             Debug.Log("Socket error:" +e.Message);
        }

        return socketReady;
    }

    private void Update()
    {
        
      
        if(socketReady)
        {
          
            if(stream.DataAvailable)
            {
                string data= reader.ReadLine();
                if(data !=null)
                 OnIncomingData(data);

            }
        }
    }

    //Seding mesages to the server
    public void Send(string data)
    {
        if(!socketReady)
        return;

        
        writer.WriteLine(data);
        writer.Flush();
      
    }

    //read messages form server
    private void OnIncomingData(string data)
    {
        Debug.Log("client:"+data);        
        string[] aData = data.Split('|');        

        switch(aData[0])
        {
            case "SWHO":
                for(int i=1; i< aData.Length-1 ; i++)
                {
                        UserConnected(aData[i],false);
                }
                Send("CWHO|"+ clientName + "|" + ((isHost)?1:0).ToString());
                break;
                
            case "SCNN":
                UserConnected(aData[1],false);
                break;

            case "Adone":
                gamemanager.Instance.otherplayerdone("Adone");                
                break;

            case "Bdone":
                gamemanager.Instance.otherplayerdone("Bdone");                
                break;

            case "Roundover":
                Aa=bool.Parse(aData[1]);  
                Ad=bool.Parse(aData[2]);  
                As=int.Parse(aData[3]);
                Ba=bool.Parse(aData[4]);  
                Bd=bool.Parse(aData[5]);  
                Bs=int.Parse(aData[6]);
                gamemanager.Instance.Roundover(Aa,Ad,As,Ba,Bd,Bs); 
                  
                Aa= false;
                Ad= false;
                Ba= false;
                Bd= false;
                Ak= false; // a input done
                Bk= false; // b input done
                     
                break;

        }
    } 

    private void UserConnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);

        if(players.Count == 2)
            GameStarter.Instance.StartGame();
    }

    private void OnApplicaitonQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
       if(!socketReady)
        return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;

    }
}








public class GameClient
{
    public string name;
    public bool isHost;
}

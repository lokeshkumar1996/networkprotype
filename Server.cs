using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO; 


public class Server : MonoBehaviour
{
   public int port = 6321;

   private List<ServerClient> clients;
   private List<ServerClient> disconnectList;

   private TcpListener server;
   private bool serverStarted;


//game variables
   public bool Aa= false;
   public bool Ad= false;
   public bool Ba= false;
   public bool Bd= false;
   public bool Ak= false; // a input done
   public bool Bk= false; // b input done
   public int As;  // score of a
   public int Bs; //score of b
   public int initialscore;

   public int attackpow = 3;
   public bool alldone;
   bool Alost =false;
   bool Blost =false;
   public string won;
   //float timer =0f;
  
//

   


   public void Init()
   { 
       DontDestroyOnLoad(gameObject);
       clients = new List<ServerClient>();
       disconnectList = new List<ServerClient>();

       

       try
       {
           server = new TcpListener(IPAddress.Any,port);
           server.Start();
           

           StartListening(); 

           serverStarted = true;
       }
       catch (Exception e)
       {
           Debug.Log("Socket error:" +e.Message);
       }
   }

   private void Update()
   {

       if(!serverStarted)
       return;

     //  timer += Time.deltaTime;

       
       
 
       foreach(ServerClient c in clients)
       {
           // is the ccinet staill connected ? 
           if(!IsConnect(c.tcp))
           {   
               c.tcp.Close();
               disconnectList.Add(c);
               continue;               
           }
           else
           {     
              NetworkStream s = c.tcp.GetStream();               
               if(s.DataAvailable)
              {  
                   StreamReader reader = new StreamReader(s, true);
                   string data = reader.ReadLine();

                   if(data !=null)                   
                        OnIncomingData(c,data);
              }
           }
       }

       for(int i=0; i<disconnectList.Count -1; i++)
       {
           //tell our player sombody has disconnected

           clients.Remove(disconnectList[i]);
           disconnectList.RemoveAt(i);
       }

      /* if(timer > 6f)
       {
           alldone = true;
           timer = 0f;
       }*/

       if(alldone)
       gamelogics();






   }

    private bool IsConnect(TcpClient c)
    {
        try
        {
            if(c!=null && c.Client != null && c.Client.Connected)
            {
                if(c.Client.Poll(0,SelectMode.SelectRead))
                    return!(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }   
   
   private void StartListening()
   {
       server.BeginAcceptTcpClient(AcceptTcpClient,server);
   }  

    private void AcceptTcpClient(IAsyncResult ar)
    { 
            TcpListener listener = (TcpListener)ar.AsyncState;

            string allUsers = "";
            foreach(ServerClient i in clients)
            {
                allUsers += i.clientName + '|';
            }

            ServerClient sc =new ServerClient(listener.EndAcceptTcpClient(ar));         

            clients.Add(sc);
           
            StartListening();

           Debug.Log("Somebody has connected!");

           
            
            Broadcast("SWHO|"+allUsers,clients[clients.Count-1]);
    }
    
    //server send
    private void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> {c};
        Broadcast(data, sc);
    }

    private void Broadcastoneclient(string data, ServerClient sc)
    {
         try
            {   
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();  
            }
            catch (Exception e)
            {
                
               Debug.Log("Write error: " +e.Message);
            }
    }

    private void Broadcast(string data, List<ServerClient> cl)
    {
        
        foreach(ServerClient sc in cl)
        {
            try
            {   
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();  
            }
            catch (Exception e)
            {
                
               Debug.Log("Write error: " +e.Message);
            }
        }
    }

    //server read
    private void OnIncomingData(ServerClient c, string data)
    {
       Debug.Log("server:"+data);  

        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.clientName=aData[1];
                c.isHost = (aData[2] == "0")? false:true;
                Broadcast("SCNN|" + c.clientName, clients);
                break;

            case "gamestarted":
                string msg = "gamestarted|";
                foreach(ServerClient cs in clients)
                {
                    msg += cs.clientName.ToString() + "|" ;      
                }
                msg +=initialscore.ToString() + "|" ;
                msg +=attackpow.ToString() ;
                Broadcast(msg, clients);
                break;

            case "Adone":   
                   
                Aa=bool.Parse(aData[1]);  
                Ad=bool.Parse(aData[2]);  
                As=int.Parse(aData[3]);  
                Ak = true;                          
                Broadcastoneclient("Adone|00",clients[clients.Count - 1 ]);      
                break;

            case "Bdone":
                
                 Ba=bool.Parse(aData[1]);  
                 Bd=bool.Parse(aData[2]);  
                 Bs=int.Parse(aData[3]);   
                 Bk = true;
                 Broadcastoneclient("Bdone|00",clients[0]);
                break;  

            case "alldone":
                alldone = true;
                break; 

            case "MsgA":
                Broadcast("MsgA|"+aData[1],clients);      
                break;

            case "MsgB":
                Broadcast("MsgB|"+aData[1],clients);     
                break;  

        }
    }



    private void gamelogics()
    {
        
       
         //selection ponits reduction
         
         if(Aa) As -= 1;
         if(Ad) As -= 1;
         if(Ba) Bs -= 1;
         if(Bd) Bs -= 1;

         //checkvictory();
         


        //calculation for attack reduction
        if (Ba == true && Aa == true && Bd == false && Ad ==false)
        {
            As = As - attackpow ;
            Bs = Bs - attackpow ;         
        }
        else
        {        
            if (Ba == true &&  Ad ==false)
            {
            As = As - attackpow ;             
            }

            if ( Aa == true && Bd == false )
            {
            Bs = Bs - attackpow ;             
            }  
        }

        checkvictory();
        //
        
        
        
        string msg = "Roundover|";
         msg += Aa.ToString() + "|" ;
         msg += Ad.ToString() + "|" ;
         msg += As.ToString() + "|" ;
         msg += Ba.ToString() + "|" ;
         msg += Bd.ToString() + "|" ;
         msg += Bs.ToString() + "|" ;
         if(Blost){msg += "A";}
         if(Alost){msg += "B";}           

        
        foreach(ServerClient c in clients)
        {
            Debug.Log("clientslist");
            Broadcast(msg, c);
        }
        

         Aa= false;
         Ad= false;
         Ba= false;
         Bd= false;
         Ak= false; // a input done
         Bk= false; // b input done
         alldone = false;
        
        
       
    }
    

    private void checkvictory()
    {
        if(As <= 0)
        {
            
            Alost = true;
        }

        if(Bs <= 0)
        {
            
            Blost = true;
        }
    }


    
    

} 







 


public class ServerClient
{
    public string clientName;  
    public TcpClient tcp ;
    public bool isHost;

    public ServerClient(TcpClient tcp)
    {
       this.tcp =tcp; 
         //this.tcp =  new TcpClient("127.0.0.1",port);
      
    }   
}
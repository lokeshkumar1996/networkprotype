using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class gamemanager : MonoBehaviour
{
    public static gamemanager Instance {set; get;}

    public Toggle toggleofAa;
    public Toggle toggleofAd;
    public Toggle toggleofBa;
    public Toggle toggleofBd;
    public Toggle toggleofAk;
    public Toggle toggleofBk;   
    public Text scoreofA;
    public Text scoreofB;
    //public Text timertext; 
    public Text Roundcounter;
    private Animator anim;
    public Animator BulletAattacked;
    public Animator BulletAdefended;
    public Animator BulletBattacked;
    public Animator BulletBdefended;
    public Text clientAname;
    public Text clientBname;
    public Text clientAnamemsg;
    public Text clientBnamemsg;
    public Text Winningmessage;

    
    public GameObject inputmessageofA;    
    public GameObject inputmessageofB;
    public GameObject Winnpannel;


    
    


    public string clientwho ;
    
    public bool Aa;
    public bool Ad;
    public bool Ba;
    public bool Bd;
    public bool Ak; // a input done
    public bool Bk;  // b input done
    public int As;  // score of a
    public int Bs; //score of b  
    public int round;
    public bool inputdone;
    private Client client;
    public string winner;
    
   // float timer;
   // float timelimit = 5f;
   // bool timerend = true;


 public void Start()
   {  
      Instance = this;   
      client = FindObjectOfType<Client>(); 

      BulletAdefended = GameObject.Find("bulletofAdefended").GetComponent<Animator>(); 
      BulletAattacked = GameObject.Find("bulletofAattacked").GetComponent<Animator>(); 
      BulletBdefended = GameObject.Find("bulletofBdefended").GetComponent<Animator>(); 
      BulletBattacked = GameObject.Find("bulletofBattacked").GetComponent<Animator>(); 
      anim = GameObject.Find("roudnfadeanim").GetComponent<Animator>(); 

      round =1;
      As= client.initialscore;
      Bs= client.initialscore;  

      Roundcounter.text = round.ToString();  
      scoreofA.text = As.ToString();
      scoreofB.text = Bs.ToString();

     if(client.isHost)
      {
          clientwho = "A";
          clientAname.text = client.clientName;
          clientBname.text = client.otherclientName;
          clientAnamemsg.text = client.clientName;
          clientBnamemsg.text = client.otherclientName;
          inputmessageofA.SetActive(true);
          inputmessageofB.SetActive(false);

      }
      else
      {
          clientwho = "B";
          clientAname.text = client.otherclientName;
          clientBname.text = client.clientName;
          clientAnamemsg.text = client.otherclientName;
          clientBnamemsg.text = client.clientName;
          inputmessageofA.SetActive(false);
          inputmessageofB.SetActive(true);
      } 

      Restictotherclients(clientwho);   
      inputdone =false;
   } 

   private void Update()
   {
      scoreofA.text = As.ToString();
      scoreofB.text = Bs.ToString();
      //timertext.text = timer.ToString();

     /*   if(timerend){
       timer += Time.deltaTime;
        }


      //timer end send
      if(timer>timelimit && timerend)
      {
         if(clientwho == "A")
        {
         string msg = "Adone|";
         msg += Aa.ToString() + "|" ;
         msg += Ad.ToString() + "|" ;
         msg += As.ToString() ; 
       
         client.Send(msg); 
         inputdone = true;
         timerend = false;
         
        }

      if(clientwho == "B")
        {
         string msg = "Bdone|";
         msg += Ba.ToString() + "|" ;
         msg += Bd.ToString() + "|" ;
         msg += Bs.ToString() ; 
        
         client.Send(msg);
         inputdone = true;
         timerend = false;
         
        }
      } */

    //after input send
    if(inputdone == false )
    {
       if(clientwho == "A" && Ak)
      {
         string msg = "Adone|";
         msg += Aa.ToString() + "|" ;
         msg += Ad.ToString() + "|" ;
         msg += As.ToString() ; 
       
         client.Send(msg); 
         inputdone = true;
       //  timerend = false;
      }

      if(clientwho == "B" && Bk)
      {
         string msg = "Bdone|";
         msg += Ba.ToString() + "|" ;
         msg += Bd.ToString() + "|" ;
         msg += Bs.ToString() ; 
        
         client.Send(msg);
         inputdone = true;
        // timerend = false;
      }

    }
   } 


    public void Restictotherclients(string data)   // get the client name and enable only its interactablilty
    {
     
     toggleofAa.GetComponent<Toggle>().interactable = true; 
     toggleofAd.GetComponent<Toggle>().interactable = true;  
     toggleofBa.GetComponent<Toggle>().interactable = true; 
     toggleofBd.GetComponent<Toggle>().interactable = true; 
     //toggleofAk.GetComponent<Toggle>().interactable = true; 
     //toggleofBk.GetComponent<Toggle>().interactable = true;
     toggleofAa.GetComponent<Toggle>().isOn = false; 
     toggleofAd.GetComponent<Toggle>().isOn = false;  
     toggleofBa.GetComponent<Toggle>().isOn = false; 
     toggleofBd.GetComponent<Toggle>().isOn = false; 
     toggleofAk.GetComponent<Toggle>().isOn = false; 
     toggleofBk.GetComponent<Toggle>().isOn = false;   
     toggleofAa.GetComponent<Toggle>().interactable = false; 
     toggleofAd.GetComponent<Toggle>().interactable = false;  
     toggleofBa.GetComponent<Toggle>().interactable = false; 
     toggleofBd.GetComponent<Toggle>().interactable = false; 
     toggleofAk.GetComponent<Toggle>().interactable = false; 
     toggleofBk.GetComponent<Toggle>().interactable = false; 
      //toggleofAk.GetComponent<Toggle>().enabled = false;
      //toggleofBk.GetComponent<Toggle>().enabled = false;    

     

        if(data == "A")
        {
        toggleofAa.GetComponent<Toggle>().interactable = true; 
        toggleofAd.GetComponent<Toggle>().interactable = true; 
        toggleofAk.GetComponent<Toggle>().interactable = true; 
         toggleofAk.GetComponent<Toggle>().enabled = true;  
        }

        if(data == "B")
        {
        toggleofBa.GetComponent<Toggle>().interactable = true; 
        toggleofBd.GetComponent<Toggle>().interactable = true; 
        toggleofBk.GetComponent<Toggle>().interactable = true;
         toggleofBk.GetComponent<Toggle>().enabled = true; 
        }

    }

    public void otherplayerdone(string data)
    {
        if(data== "Adone")
        {
            toggleofAk.GetComponent<Toggle>().interactable = true;
            toggleofAk.GetComponent<Toggle>().isOn = true;
            toggleofAk.GetComponent<Toggle>().interactable = false;
            if(Bk)
            {
              client.Send("alldone");
            }
        }

        if(data== "Bdone")
        {
            toggleofBk.GetComponent<Toggle>().interactable = true;
            toggleofBk.GetComponent<Toggle>().isOn = true;
            toggleofBk.GetComponent<Toggle>().interactable = false;
            
            if(Ak)
            {
              client.Send("alldone");
            }

            
        }
    }
  
    public void Roundover(bool AAa, bool AAd, int AAs, bool BBa, bool BBd, int BBs,string won)
    {      
        toggleofBa.GetComponent<Toggle>().interactable = true; 
        toggleofBd.GetComponent<Toggle>().interactable = true;  
        toggleofAa.GetComponent<Toggle>().interactable = true; 
        toggleofAd.GetComponent<Toggle>().interactable = true; 
        

        toggleofAa.GetComponent<Toggle>().isOn = AAa;
        toggleofAd.GetComponent<Toggle>().isOn = AAd;
        toggleofBa.GetComponent<Toggle>().isOn = BBa;
        toggleofBd.GetComponent<Toggle>().isOn = BBd;
        As = AAs;
        Bs = BBs;

        toggleofBa.GetComponent<Toggle>().interactable = false; 
        toggleofBd.GetComponent<Toggle>().interactable = false;  
        toggleofAa.GetComponent<Toggle>().interactable = false; 
        toggleofAd.GetComponent<Toggle>().interactable = false;

          if(AAa)
          {                        
             
            if(AAa && BBd)
            {             
            BulletAdefended.SetBool("bulletA",true);    
            }
            if(AAa && BBd == false)
            {            
            BulletAattacked.SetBool("bulletA",true);          
            }
          }    

          if(BBa)
          {                        
             
            if(BBa && AAd)
            {             
            BulletBdefended.SetBool("bulletB",true);    
            }
            if(BBa && AAd == false)
            {            
            BulletBattacked.SetBool("bulletB",true);          
            }
          }       

        scoreofA.text = AAs.ToString();
        scoreofB.text = BBs.ToString();

        if(won =="A" || won =="B")
        {
          winner = won;
          Invoke("winning", 2.2f);  
        }
        else
        {
          Invoke("nextround", 2.2f);
        }
        
       //Debug.Log("roundover for all");
    }
    

    public void winning()
    {
      Winnpannel.SetActive(true);
      if(winner == "A")
      {
        string msg = clientAname.text;
         msg += " Won" ; 
         Winningmessage.text = msg;        
      }
      if(winner == "B")
      {
        string msg = clientBname.text;
         msg += " Won" ;
         Winningmessage.text = msg;
      } 
    }


    private void nextround()
    {
        //Destroy(BulletA);
        round = round+ 1;
        Roundcounter.text = round.ToString(); 
        BulletAdefended.SetBool("bulletA",false);  
        BulletAattacked.SetBool("bulletA",false); 
        BulletBdefended.SetBool("bulletB",false);  
        BulletBattacked.SetBool("bulletB",false);     

        anim.SetBool("playfade",true);
        Invoke("stopanim",1.5f);

        
    }

    private void stopanim()
    {
        anim.SetBool("playfade",false);
        Restictotherclients(clientwho);
       // timer= 0f;
      //  timerend =true;
        inputdone = false;
    }

    

    public void mainmenubutton()
    {
      client.OnApplicaitonQuit();

      if(GameObject.Find("GameStarter") != null)
      {
        Destroy(GameObject.Find("GameStarter"));
      }
      if(GameObject.Find("Client") != null)
      {
        Destroy(GameObject.Find("Client"));
      }
      if(GameObject.Find("Server") != null)
      {
        Destroy(GameObject.Find("Server"));
      }
      
      SceneManager.LoadScene("menuserver");
    }
    

// toggle enabling

    public void Aattack()
    {
        if(As > 0)
        {        
        Aa = toggleofAa.GetComponent<Toggle>().isOn; 
        if(Aa) { As -= 1;}
        if(!Aa) { As += 1;}
        }             
    }

    public void Adefence()
    {
       if(As > 0)
        {        
        Ad = toggleofAd.GetComponent<Toggle>().isOn; 
        if(Ad) { As -= 1;}
        if(!Ad) { As += 1;}
        }
    }

    public void Battack()
    {
        if(Bs > 0)
        {        
        Ba =  toggleofBa.GetComponent<Toggle>().isOn;
        if(Ba) { Bs -= 1;}
        if(!Ba) { Bs += 1;}
        } 
    }

     public void Bdefence()
    {
       if(Bs > 0)
        {        
        Bd =  toggleofBd.GetComponent<Toggle>().isOn;
        if(Bd) { Bs -= 1;}
        if(!Bd) { Bs += 1;}
        }
    }

    public void Ainputdone()
    {
     
       Ak =  toggleofAk.GetComponent<Toggle>().isOn;
       toggleofAk.GetComponent<Toggle>().interactable = false; 
       toggleofAa.GetComponent<Toggle>().interactable = false; 
       toggleofAd.GetComponent<Toggle>().interactable = false; 
    }

     public void Binputdone()
    {
      
       Bk =  toggleofBk.GetComponent<Toggle>().isOn; 
       toggleofBk.GetComponent<Toggle>().interactable = false;
       toggleofBa.GetComponent<Toggle>().interactable = false; 
       toggleofBd.GetComponent<Toggle>().interactable = false; 
    }

//


}

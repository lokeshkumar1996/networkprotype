using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatmanager : MonoBehaviour
{
    public static Chatmanager Instance {set; get;}

    public Button sendA;
    public Button sendB;
    public InputField messageAinput;
    public InputField messageBinput;
    public Text messageofA;
    public Text messageofB;

    private Client client;

    // Start is called before the first frame update
    void Start()
    {
    Instance = this;   
    client = FindObjectOfType<Client>(); 
        
	sendA.onClick.AddListener(ShareTextA);
    sendB.onClick.AddListener(ShareTextB);
    }

    private void ShareTextA()
    {
        
        string data = "MsgA|";
        data += messageAinput.text;
        client.Send(data);
        messageAinput.text = "";
    }

     private void ShareTextB()
    {
        string data = "MsgB|";
        data += messageBinput.text;
        client.Send(data);
        messageBinput.text = "";
    }

    public void ShowmessageofA(string data)
    {
        Debug.Log("chatmangaere messageA:"+data);
        messageofA.text = data;
    }

    public void ShowmessageofB(string data)
    {
        Debug.Log("chatmangaere messageB:"+data);
        messageofB.text = data;
    }

    public void clearallmessages()
    {
        messageofB.text = "";
        messageofA.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

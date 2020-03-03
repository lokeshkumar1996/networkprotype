using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;


public class sendIP : MonoBehaviour
{
  public  Button shareButton;
  public  Text ipdisplay;
  public  Text connectto;

	private bool isFocus = false;
	private bool isProcessing = false;

    private string serverIP; 
	void  Start () {
		shareButton.onClick.AddListener (ShareText);
	}

	void OnApplicationFocus (bool focus) {
		isFocus = focus;
	}

	private void ShareText () {

        serverIP = GetLocalIPAddress();
        Debug.Log("server ip:"+ serverIP);
        ipdisplay.text = serverIP;        
        connectto.text = "Connect to:";


	   #if UNITY_ANDROID
		if (!isProcessing) {
			StartCoroutine (ShareTextInAnroid ());
		}
		#else
		Debug.Log("No sharing set up for this platform.");
		#endif
	}

     public static string GetLocalIPAddress()
        {
              var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
            
        }
        //throw new Exception("No network adapters with an IPv4 address in the system!");
        return "cantread IP";

        }



	#if UNITY_ANDROID
	public IEnumerator ShareTextInAnroid () {

        
		var shareSubject = "Join my game";
		var shareMessage = "type my IP in connect field : "+ serverIP +" \n and connect to my game";
		                   
						   
        Debug.Log(shareMessage);
        
		isProcessing = true;

		if (!Application.isEditor) {
			//Create intent for action send
			AndroidJavaClass intentClass = 
				new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = 
				new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> 
				("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));

			//put text and subject extra
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), shareSubject);
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), shareMessage);

			//call createChooser method of activity class
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = 
				unity.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject chooser = 
				intentClass.CallStatic<AndroidJavaObject> 
				("createChooser", intentObject, "Invite your friend");
			currentActivity.Call ("startActivity", chooser);
		}

		yield return new WaitUntil (() => isFocus);
		isProcessing = false;
	}
	#endif
}

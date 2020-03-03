using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyinsec : MonoBehaviour
{
    // Start is called before the first frame update
   public float timerinsec = 5f;
    float elapsed = 0;
    

    // Update is called once per frame
    public void FixedUpdate()
    {
        // Debug.Log("time"); 
        elapsed += Time.deltaTime;

        if (elapsed >= timerinsec)
        {

             Destroy(gameObject);
    
        }    
    }
}

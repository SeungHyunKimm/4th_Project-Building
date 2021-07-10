using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCommend : MonoBehaviour
{
    public void OnVoiceFront()
    {
            transform.eulerAngles = new Vector3(0, 0, 0);
         if (gameObject.name.Contains("Floor"))
        {
            transform.eulerAngles = new Vector3(90, 0, 0);
        }
    } 
    
    public void OnVoiceLeft()
    {
        transform.eulerAngles = new Vector3(0, -90, 0);
        if (gameObject.name.Contains("Floor"))
        {
            transform.eulerAngles  = new Vector3(90, -90, 0);
        }
    }  
    
    public void OnVoiceRight()
    {
        transform.eulerAngles = new Vector3(0, 90, 0);
        if (gameObject.name.Contains("Floor"))
        {
            transform.eulerAngles = new Vector3(90, 90, 0);
        }
    }

    public void OnVoiceBack()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);
        if (gameObject.name.Contains("Floor"))
        {
            transform.eulerAngles = new Vector3(90, -180, 0);
        }
    }
}

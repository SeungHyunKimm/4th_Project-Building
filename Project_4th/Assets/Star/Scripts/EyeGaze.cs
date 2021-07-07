using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGaze : MonoBehaviour
{
    bool isLook = false;

    public void OnLook() {
        isLook = true;
    }  
    public void OnLookAway() {
        isLook = false;
    }


    public void OnFront() {
        transform.localEulerAngles += new Vector3(0, 90, 0);
        print("front?");
    }

    public void OnVoiceFront()
    {
        if (isLook)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (isLook && gameObject.name.Contains("Floor"))
        {
            transform.localEulerAngles = new Vector3(90, 0, 0);
        }
    } 
    
    public void OnVoiceLeft()
    {
        if (isLook) {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
        else if (isLook && gameObject.name.Contains("Floor"))
        {
            transform.localEulerAngles = new Vector3(90, -90, 0);
        }
    }  
    
    public void OnVoiceRight()
    {
        if (isLook) {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (isLook && gameObject.name.Contains("Floor"))
        {
            transform.localEulerAngles = new Vector3(90, 90, 0);
        }
    }

    public void OnVoiceBack()
    {
        if (isLook)
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
        else if (isLook && gameObject.name.Contains("Floor"))
        {
            transform.localEulerAngles = new Vector3(90, -180, 0);
        }
    }
}

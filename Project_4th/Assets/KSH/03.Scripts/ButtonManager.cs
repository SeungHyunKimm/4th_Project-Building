using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    GameObject FF;
    GameObject SF;
    GameObject TF;
    void Start()
    {
        FF = GameObject.Find("FirstFloor");
        SF = GameObject.Find("SecondFloor");
        TF = GameObject.Find("ThirdFloor");
    }
    public void OnClick_FirstFloor()
    {
        SF.SetActive(false);
        TF.SetActive(false);
    }
    public void OnClick_SecondFloor()
    {
        FF.SetActive(false);
        TF.SetActive(false);
        SF.transform.position = new Vector3(0, 0, 0);
    }
    public void OnClick_ThirdFloor()
    {
        FF.SetActive(false);
        SF.SetActive(false);
        TF.transform.position = new Vector3(0, 0, 0);
    }
    public void OnClick_Reset()
    {
        FF.SetActive(true);
        SF.SetActive(true);
        TF.SetActive(true);

        FF.transform.position = new Vector3(0, 0, 0);
        SF.transform.position = new Vector3(0, 2, 0);
        TF.transform.position = new Vector3(0, 4, 0);
    }
}

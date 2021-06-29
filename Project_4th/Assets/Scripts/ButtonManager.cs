using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    GameObject FF;
    GameObject SF;
    GameObject TF;
    public Transform FF_pos;
    public Transform SF_pos;
    public Transform TF_pos;
    void Start()
    {

        FF = GameObject.Find("FirstFloor");
        SF = GameObject.Find("SecondFloor");
        TF = GameObject.Find("ThirdFloor");

        FF_pos = GameObject.Find("FirstFloor").transform;
        SF_pos = GameObject.Find("SecondFloor").transform;
        TF_pos = GameObject.Find("ThirdFloor").transform;

    }

    void Update()
    {
        


    }


    public void OnClick_FirstFloor()
    {
        //2층 비활성화
        //3층 비활성화
        SF.SetActive(false);
        TF.SetActive(false);
    }
    public void OnClick_SecondFloor()
    {
        FF.SetActive(false);
        TF.SetActive(false);
        SF_pos.transform.position = new Vector3(0, 0, 0);
    }
    public void OnClick_ThirdFloor()
    {
        FF.SetActive(false);
        SF.SetActive(false);
        TF_pos.transform.position = new Vector3(0, 0, 0);
    }

    public void OnClick_Reset()
    {
        FF.SetActive(true);
        SF.SetActive(true);
        TF.SetActive(true);

        FF_pos.transform.position = new Vector3(0, 0, 0);
        SF_pos.transform.position = new Vector3(0, 2, 0);
        TF_pos.transform.position = new Vector3(0, 4, 0);
    }
}

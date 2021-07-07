using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{

    Base bs;


    void OnTriggerEnter(Collider other)
    {
        print("충돌 감지 완료");
        if (other.transform.name == "Mini")
        {
            other.transform.position = Vector3.one * 1000;
            other.gameObject.SetActive(false);
            //Destroy(other);
        }
        else { 
        bs = GameObject.Find("Base").GetComponent<Base>();
        bs.OnClickDestroy(other.gameObject);
        }

    }

    
}
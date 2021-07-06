using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{

    Base bs;


    void OnCollisionEnter(Collision collision)
    {
        bs = GameObject.Find("Base").GetComponent<Base>();
        bs.OnClickDestroy(collision.gameObject);
    }

}
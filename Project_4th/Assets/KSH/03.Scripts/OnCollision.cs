using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{

    Base bs;
    void OnCollisionEnter(Collision collision)
    {
        print("충돌 감지 완료");

        bs = GameObject.Find("Base").GetComponent<Base>();
        bs.OnClickDestroy(collision.gameObject);
    }

}
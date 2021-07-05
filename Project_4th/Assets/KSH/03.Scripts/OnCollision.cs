using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        //나와 부딪힌 놈을 파괴하자
        Destroy(collision.gameObject);
    }
}

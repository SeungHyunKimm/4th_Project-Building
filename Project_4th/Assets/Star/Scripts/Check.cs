using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name.Contains("Floor")
            || collision.gameObject.name.Contains("Ground")
            ) {
           StartCoroutine(drop());
        }
    }

    IEnumerator drop() {
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(2);
        rb.isKinematic = true;
    }

}

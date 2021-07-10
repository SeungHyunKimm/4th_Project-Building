using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("theSpot"))
       {
            Vector3 pos = other.transform.position;
            transform.position = new Vector3(pos.x, pos.y+.01f, pos.z);
            other.transform.parent.SetParent(transform);
        }
    }
}

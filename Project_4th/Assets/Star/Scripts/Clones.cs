using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clones : MonoBehaviour
{
    void Start()
    {
        
    }

    public void OnClickClone() {

        GameObject a = Instantiate(gameObject);
        a.transform.position = Vector3.one*.1f;
        a.transform.localScale = transform.localScale * .1f;
    }
}

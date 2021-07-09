using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_Test : MonoBehaviour
{
    int blank = 0;
    float R;
    float G;
    float B;

    private void Start()
    {

    }

    public void OnclickButtonToLightColor()
    {
        blank++;
    }

    public void Update()
    {


        if (blank == 1)
        {
            Light lg = GetComponent<Light>();
            lg.color = new Color(50, 50, 50);
        }

        if(blank == 2)
        {
            Light lg = GetComponent<Light>();
            lg.color = new Color(62, 199, 41);

            if (blank > 3)
            {
                blank = 0;
            }

        }


    }
}

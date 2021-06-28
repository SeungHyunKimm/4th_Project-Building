using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginPos : MonoBehaviour
{
    public Transform[] furnitures;
     Vector3[] originPos;
     Quaternion[] originRot;
    void Start()
    {
        originPos = new Vector3[furnitures.Length];
        originRot = new Quaternion[furnitures.Length];
        for (int i = 0; i < furnitures.Length; i++)
        {
         originPos[i] = furnitures[i].position;
         originRot[i] = furnitures[i].rotation;
        }
    }
        public void Reset_Position() {
            for (int i = 0; i < furnitures.Length; i++)
        {
            furnitures[i].position = originPos[i];
            furnitures[i].rotation = originRot[i];
        }
    }

    public void EyeGazing(Transform obj)
        {
        Vector3 originScale = obj.localScale;
        if (obj.localScale.x != .2f) { obj.localScale = originScale * 2; }
        else { obj.localScale = originScale / 2;}
        }
}

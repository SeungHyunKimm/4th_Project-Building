using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Threading.Tasks;


public class BdTool : MonoBehaviour
{
    // 협소주택 모델링 obj
    public GameObject buildingA;
    // 협소주택 내부 꾸며주는 벽, 가구 obj 배열 목록
    public GameObject[] walls;
    public GameObject[] furnitures;
    public GameObject[] products;
    // 복제 하기 위해 선택한 obj의 ID(위 3개 배열의 번호대로)
    

    public Transform building;
    public GameObject[] floor;
    public float[] floorY;
    public bool[] isOnOff;
    void Start()
    {
        
    }

}

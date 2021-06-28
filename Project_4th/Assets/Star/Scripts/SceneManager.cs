using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    int objID;
    //선택할 맵이 프로젝트 안에 프리펩으로 있고 프리펩 종류를 담는 변수
    public GameObject[] buildingObjs;
     void Awake()
    {
        objID = Building.instance.objID;
    }

    public void OnClickBuilding() {
        //포톤 접속 시 정해지는 int 값과 배열 상 같은 번호의 오브젝트가 복사된다.
        Instantiate(buildingObjs[objID]);
    }
}

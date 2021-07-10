using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{

    Base bs;


    void OnTriggerEnter(Collider other)
    {
        
        bs = GameObject.Find("Base(Clone)").GetComponent<Base>();

        if (other.transform.name.Contains("Mini"))
        {
            // 미니어쳐랑 부딧치면 비활성화만 하자
            //other.transform.position = Vector3.one * 1000;
            //other.gameObject.SetActive(false);
            //Destroy(other);
            //비활성화 대신 포톤네트워크로 삭제해준다.
            //아래 게임오브젝트 받는 함수랑은 overload로 차이 둠
            bs.OnClickDestroy(other);
        }
        // 플레이어나 메뉴랑 부딪히면 작동안하기
        else if (other.transform.name.Contains("Menu") 
            && other.transform.name.Contains("User")
            && other.transform.name.Contains("Ground")
            ) { }
        else {
            // 그 외에는 데이터랑 같이 비활성화하자
            // 데이터랑 같이 포톤네트워크로 삭제해준다.
            bs.OnClickDestroy(other.gameObject);
        }

        print("충돌 감지 완료");
    }

    
}
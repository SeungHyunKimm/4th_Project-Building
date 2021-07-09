using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveFloor : MonoBehaviour
{
    // 최초의 위치값(여기서는 1층이 된다.)
    float firstdir;
    float seconddir;
    float thirddir;
    //float fourthdir;

    // 개별 오브젝트의 위치값(옮겨질 1층 외의 다른 층들)
    public Transform firstBlock;
    public Transform secondBlock;
    public Transform thirdBlock;

    //4층 - 추후에 업데이트예정
    //public Transform fourthBlock;

    Vector3 dir;

    int ButtonCount;


    
    // 1단계
    // 만약 블록 1이 사라지면(셋엑티브펄스) 블록 2를 1의 자리로 위치시켜라(블록 1이 사라지면 블록1의 y값을 가져온다.)
    // 만약 블록 2가 사라지면(셋엑티브펄스) 블록 3을 2의 자리로 위치시켜라
    // 만약 블록 3이 사라지면(셋엑티브펄스) 블록 4를 3의 자리로 위치시켜라


    // 2단계
    // 버튼을 눌렀을 시 활성화된다.


    void Start()
    {
        // 최초의 위치값. dir에 y의 값을 할당한다.
        firstdir = firstBlock.position.y;
        seconddir = secondBlock.position.y;
        thirddir = thirdBlock.position.y;

        //4층 - 추후에 업데이트 예정
        //fourthdir = fourthBlock.position.y;
    }
    void OnButtonClickFloorSetActiveFalse()
    {
        ButtonCount++;
    }


    public void OnClick_SetButton()
    {
        PhotonView pv = GetComponent<PhotonView>();
        pv.RPC("RPCSetUp", RpcTarget.All);
    }

    [PunRPC]
    void RPCSetUp() {

        ButtonCount++;
        if (ButtonCount == 1)
        {
            firstBlock.gameObject.SetActive(false);

        }

        else if (ButtonCount == 2)
        {
            secondBlock.gameObject.SetActive(false);

        }

        else if (ButtonCount == 3)
        {
            thirdBlock.gameObject.SetActive(false);
        }

        //4층 추후에 업데이트 예정
        //else if(ButtonCount == 4)
        //    {
        //    fourthBlock.gameObject.SetActive(false);
        //    }

        //카운트 횟수가 4번 이상 되면 초기화를 시킨다.
        else
        {
            ButtonCount = 0;
            firstBlock.gameObject.SetActive(true);
            secondBlock.gameObject.SetActive(true);
            thirdBlock.gameObject.SetActive(true);



        }

        if (firstBlock.gameObject.activeSelf == false)
        {
            secondBlock.position = new Vector3(secondBlock.position.x, firstdir, secondBlock.position.z);
            thirdBlock.position = new Vector3(thirdBlock.position.x, seconddir, thirdBlock.position.z);
            //fourthBlock.position = new Vector3(fourthBlock.position.x, thirddir, fourthBlock.position.z);
        }
        else
        {
            secondBlock.position = new Vector3(secondBlock.position.x, seconddir, secondBlock.position.z);
            thirdBlock.position = new Vector3(thirdBlock.position.x, thirddir, thirdBlock.position.z);
        }
        if (secondBlock.gameObject.activeSelf == false)
        {
            thirdBlock.position = new Vector3(thirdBlock.position.x, firstdir, thirdBlock.position.z);
            //fourthBlock.position = new Vector3(fourthBlock.position.x, seconddir, fourthBlock.position.z);
        }
        else
        {
            if (firstBlock.gameObject.activeSelf == true)
            {
                thirdBlock.position = new Vector3(thirdBlock.position.x, thirddir, thirdBlock.position.z);
            }

        }
        if (thirdBlock.gameObject.activeSelf == false)
        {
            //fourthBlock.position = new Vector3(fourthBlock.position.x, firstdir, fourthBlock.position.z);
        }


        //if (fourthBlock.gameObject.activeSelf == false)
        //{
        //}
    }

    public void OnClick_FloorReset()
    {

        //1,2,3층을 다시 활성화시킨다.
        firstBlock.gameObject.SetActive(true);
        secondBlock.gameObject.SetActive(true);
        thirdBlock.gameObject.SetActive(true);


        //1,2,3층의 위치를 초기화시켜준다.

        firstBlock.position = new Vector3(firstBlock.position.x, firstdir, firstBlock.position.z);
        secondBlock.position = new Vector3(secondBlock.position.x, seconddir, secondBlock.position.z);
        thirdBlock.position = new Vector3(thirdBlock.position.x, thirddir, thirdBlock.position.z);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void Start()
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

    void Update()
    {

        

            if (ButtonCount == 1)
                {
                firstBlock.gameObject.SetActive(false);
                }
            else if(ButtonCount == 2)
                {
                secondBlock.gameObject.SetActive(false);
                }
            else if(ButtonCount == 3)
                {
                thirdBlock.gameObject.SetActive(false);
                }
            //4층 추후에 업데이트 예정
            //else if(ButtonCount == 4)
            //    {
            //    fourthBlock.gameObject.SetActive(false);
            //    }
            else
                {
                ButtonCount = 0;
                }

        
            if (firstBlock.gameObject.activeSelf == false)
            {
                secondBlock.position = new Vector3(secondBlock.position.x, firstdir, secondBlock.position.z);
                thirdBlock.position = new Vector3(thirdBlock.position.x, seconddir, thirdBlock.position.z);
                //fourthBlock.position = new Vector3(fourthBlock.position.x, thirddir, fourthBlock.position.z);
            }

  
            if (secondBlock.gameObject.activeSelf == false)
            {
                thirdBlock.position = new Vector3(thirdBlock.position.x, firstdir, thirdBlock.position.z);
                //fourthBlock.position = new Vector3(fourthBlock.position.x, seconddir, fourthBlock.position.z);

            }

            if (thirdBlock.gameObject.activeSelf == false)
            {
                //fourthBlock.position = new Vector3(fourthBlock.position.x, firstdir, fourthBlock.position.z);
            }

            //if (fourthBlock.gameObject.activeSelf == false)
            //{
            //}
    }
}

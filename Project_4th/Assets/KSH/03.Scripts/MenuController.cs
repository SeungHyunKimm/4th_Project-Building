﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    //메인메뉴를 클릭하게 되면
    //자기자신(MainMenu)는 없어지고 SubMenu 8가지가 나타나게 된다.
    GameObject SubMenu;
    GameObject MainMenu;
    GameObject EditMenu;
    GameObject EditScale;
    GameObject FurnitureMenu;


    GameObject Wall_Plate;
    GameObject Furniture_Plate;
    GameObject Product_Plate;

    //쓰레기통
    GameObject TrashBin;

    int editMenuCnt = 0;
    int editScaleCnt = 0;
    int furnitureMenuCnt = 0;

    void Start()

    {
        SubMenu = GameObject.Find("SubMenu");
        MainMenu = GameObject.Find("MainMenu");
        EditMenu = GameObject.Find("EditMenu");
        EditScale = GameObject.Find("EditScale");
        FurnitureMenu = GameObject.Find("FurnitureMenu");

        //가구 프리팹 관련 메뉴 설정

        Wall_Plate = GameObject.Find("Wall_Plate");
        Furniture_Plate = GameObject.Find("Furniture_Plate");
        Product_Plate = GameObject.Find("Product_Plate");

        //쓰레기통
        TrashBin = GameObject.Find("Collision_TrashBin");


        SubMenu.SetActive(false);
        EditMenu.SetActive(false);
        EditScale.SetActive(false);
        FurnitureMenu.SetActive(false);
        Wall_Plate.SetActive(false);
        Furniture_Plate.SetActive(false);
        Product_Plate.SetActive(false);
        TrashBin.SetActive(false);

    }


    public void OnClickMainMenu()
    {
        //서브메뉴 8가지가 나타난다.
        SubMenu.SetActive(true);
        //자기자신은 없어지고
        MainMenu.SetActive(false);
    }

    public void OnClickEditMenu()
    {
        //EditMenu 활성화
        EditMenu.SetActive(true);
        if (EditMenu.activeSelf == true)
        {
            editMenuCnt++;
        }
        if (editMenuCnt == 2)
        {
            EditMenu.SetActive(false);
            editMenuCnt = 0;
        }
    }

    public void OnClickEditScale()
    {
        EditScale.SetActive(true);
        if (EditScale.activeSelf == true)
        {
            editScaleCnt++;
        }
        if (editScaleCnt == 2)
        {
            EditScale.SetActive(false);
            editScaleCnt = 0;
        }
    }

    public void OnClickFurnitureMenu()
    {
        FurnitureMenu.SetActive(true);
        if (FurnitureMenu.activeSelf == true)
        {
            furnitureMenuCnt++;
        }
        if (furnitureMenuCnt == 2)
        {
            FurnitureMenu.SetActive(false);
            furnitureMenuCnt = 0;
        }
    }

    //가구 프리팹에 대한 3가지 메뉴
    public void OnClickWall_PlateMenu()
    {
        Wall_Plate.SetActive(true);
    }
    public void OnClickFurniture_PlateMenu()
    {
        Furniture_Plate.SetActive(true);
    }
    public void OnClickProduct_PlateMenu()
    {
        Product_Plate.SetActive(true);
    }
    //가구 프리팹 메뉴 모두 비활성화
    public void OnClickAll_PlateClose()
    {
        Wall_Plate.SetActive(false);
        Furniture_Plate.SetActive(false);
        Product_Plate.SetActive(false);
    }




}


public class CreateFurniture
{
    //여기서는 가구 프리팹 만드는 함수 만들고 버튼에 적용한다.
    public void OnClickCreateCurtain()
    {
        
    }
}
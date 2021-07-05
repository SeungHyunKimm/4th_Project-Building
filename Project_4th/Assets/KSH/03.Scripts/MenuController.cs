using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    //메인메뉴를 클릭하게 되면
    //자기자신(MainMenu)는 없어지고 SubMenu 8가지가 나타나게 된다.
    GameObject SubMenu;
    GameObject MainMenu;
    GameObject EditMenu;
    GameObject FurnitureSlate;
    GameObject EditScale;
    GameObject FurnitureMenu;

    int editMenuCnt = 0;
    int editScaleCnt = 0;
    int furnitureMenuCnt = 0;

    void Start()

    {
        SubMenu = GameObject.Find("SubMenu");
        MainMenu = GameObject.Find("MainMenu");
        EditMenu = GameObject.Find("EditMenu");
        FurnitureSlate = GameObject.Find("FurnitureSlate");
        EditScale = GameObject.Find("EditScale");
        FurnitureMenu = GameObject.Find("FurnitureMenu");

        SubMenu.SetActive(false);
        EditMenu.SetActive(false);
        EditScale.SetActive(false);
        FurnitureSlate.SetActive(false);
        FurnitureMenu.SetActive(false);
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
        if(EditMenu.activeSelf == true)
        {
            editMenuCnt++;
        }
        if(editMenuCnt == 2)
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
        if(editScaleCnt == 2)
        {
            EditScale.SetActive(false);
            editScaleCnt = 0;
        }
    }

    public void OnClickFurnitureMenu()
    {
        FurnitureMenu.SetActive(true);
        if(FurnitureMenu.activeSelf == true)
        {
            furnitureMenuCnt++;
        }
        if(furnitureMenuCnt == 2)
        {
            FurnitureMenu.SetActive(false);
            furnitureMenuCnt = 0;
        }
    }
    
    
    public void OnClickFurnitureSlate()
    {
        //FurnitureSlate 활성화
        FurnitureSlate.SetActive(true);
    }

}

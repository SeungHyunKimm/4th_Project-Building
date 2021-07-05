using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    



public class MenuController : MonoBehaviour
{
    //메인메뉴를 클릭하게 되면
    //자기자신(MainMenu)는 없어지고 SubMenu 8가지가 나타나게 된다.

    public GameObject SubMenu;
    public GameObject MainMenu;



    public void OnClickMainMenu()
    {
        //자기자신은 없어지고
        MainMenu.SetActive(false);
        //서브메뉴 8가지가 나타난다.
        SubMenu.SetActive(true);



    }

}

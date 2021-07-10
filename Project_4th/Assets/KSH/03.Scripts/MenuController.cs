using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MRTK.Tutorials.MultiUserCapabilities;


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
    int trashbinCnt = 0;

    //베이스스크립트
    Base BS;

    void Start()

    {
        //SubMenu = GameObject.Find("SubMenu");
        //MainMenu = GameObject.Find("MainMenu");
        //EditMenu = GameObject.Find("EditMenu");
        //EditScale = GameObject.Find("EditScale");
        //FurnitureMenu = GameObject.Find("FurnitureMenu");

        ////가구 프리팹 관련 메뉴 설정

        //Wall_Plate = GameObject.Find("Wall_Plate");
        //Furniture_Plate = GameObject.Find("Furniture_Plate");
        //Product_Plate = GameObject.Find("Product_Plate");

        ////쓰레기통
        //TrashBin = GameObject.Find("Collision_TrashBin");


        SubMenu = transform.GetChild(1).gameObject;
        MainMenu = transform.GetChild(0).gameObject;
        EditMenu = transform.GetChild(2).gameObject;
        EditScale = transform.GetChild(3).gameObject;
        FurnitureMenu = transform.GetChild(4).gameObject;

        //가구 프리팹 관련 메뉴 설정

        Wall_Plate = transform.GetChild(5).gameObject;
        Furniture_Plate = transform.GetChild(6).gameObject;
        Product_Plate = transform.GetChild(7).gameObject;

        //쓰레기통
        TrashBin = transform.GetChild(8).gameObject;


        SubMenu.SetActive(false);
        EditMenu.SetActive(false);
        EditScale.SetActive(false);
        FurnitureMenu.SetActive(false);
        Wall_Plate.SetActive(false);
        Furniture_Plate.SetActive(false);
        Product_Plate.SetActive(false);
        TrashBin.SetActive(false);

        StartCoroutine("FindBS");
    }

    //베이스의 베이스 스크립트 찾기
    IEnumerator FindBS() {

        while (GameObject.FindWithTag("Base") == null )
        {
            yield return null;
        }
        //가구 프리팹 배열 선언
        GameObject bs = GameObject.FindWithTag("Base");
        //print(bs.name);
        BS = bs.GetComponent<Base>();
        // 포톤뷰 연동해서 rpc처리하기
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //rpc처리하기 임포트 시 데이터 못불러옴 
            OnClickImport();
        }
    }

    public void OnClickImport() {
        BS.OnClickImportData();
    }

    public void OnClickExport()
    {
        BS.OnClickExportData();
    }

    public void OnClickDelete()
    {
        BS.OnClickDelete();
    }

    public void OnClickFloor()
    {
        MoveFloor mf = BS.transform.GetComponent<MoveFloor>();
        mf.OnClick_SetButton();
    }

    public void OnClickGetShared() {
        //메뉴컨트롤러의 부모가 포톤로케이션
        //포톤 써야해서 베이스 스크립트로 이동시킴
        BS.GetShared(transform.parent);
    }

    public void OnClickMini() {
        BS.OnClickMini(SubMenu.transform);
    }

    public void OnClickX()
    {
        BS.OnClickScaleXTotal();
    }

    public void OnClickY()
    {
        BS.OnClickScaleYTotal();
    }
    
    public void OnClickZ()
    {
        BS.OnClickScaleZTotal();
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

    public void TrashBinActivate()
    {
        TrashBin.SetActive(true);
        if(TrashBin.activeSelf == true)
        {
            trashbinCnt++;
        }
        if(trashbinCnt == 2)
        {
            TrashBin.SetActive(false);
            trashbinCnt = 0;
        }
    }



    public void OnClickMake(GameObject a)
    {
        // 3숫자 배열 정해 아이템 생성하기
        // 종류, 모델, 층(층은 0이면 활성화된걸로 알아서 귀속됨)
        int[] idx = { 0, 0, 0 };
        
        //버튼a의 계층번호와 프리펩 번호순을 같게 세팅함
        idx[1] = a.transform.GetSiblingIndex();

        //종류 구분을 위해 버튼의 부모부모에 각자 타입에 맞는 이름 넣어줌
        Transform type = a.transform.parent.parent;

        if (type.name.Contains("Wall"))
        {
            idx[0] = 0;
        }
        else if (type.name.Contains("Furniture"))
        {
            idx[0] = 1;
        }
        else
        {
            idx[0] = 2;
        }

        //그 중에도 레이아웃으로 버튼이 따로 묶인게 있어 구별하기
        if(a.transform.parent.GetSiblingIndex() == 2)
        {
            Transform firstP = type.GetChild(1);
            // 프리펩 순서와 맞추기 위해 그전 버튼의 총갯수 구해 더해주기
            int b = firstP.childCount;
            idx[1] = a.transform.GetSiblingIndex()+b;
        }

        Vector3 pos = transform.position + Vector3.forward*.5f;
        BS.OnClickCreate(idx, pos, Vector3.zero, Vector3.one);
    }

    
  
}



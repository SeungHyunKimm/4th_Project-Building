using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

#region Json 클래스 선언

//모든 obj의 데이터를 가진 클래스
[Serializable]
public class ObjInfo
{
    //타입 / 모델
    public int[] objidx = new int[3];
    public Vector3 pos; // localposition
    public Vector3 rot; // eularangle 사용할 것
    public Vector3 scale;
}

// ObjInfo = 데이터를 담는 (리스트)
// 고객마다 데이터 리스트가 다르다.
// 최종적으로 json으로 변환할 녀석
[Serializable]
public class ObjectData
{
    public List<ObjInfo> info = new List<ObjInfo>();
}

// 현재 사용 안함
[Serializable]
public class UserData
{
    public string userID;
    public List<ObjectData> data = new List<ObjectData>();
}


#endregion

// BdObj cs를 확장한다 >> 해당 스크립트 속 속성값 바꿀거라고 선언함
// 기즈모 체크 해제하면 에디터 안먹힘
[CustomEditor(typeof(BdTool))]
public class BdEditor : Editor
{
    #region Json용 변수
    static List<ObjInfo> objInfo = new List<ObjInfo>();
    static List<ObjectData> objdata = new List<ObjectData>();
    //마우스로 오브젝트 제거 시 ObjInfo도 제거해줘야 해서 순번 확인으로 만드는 gameobj 리스트
    static List<GameObject> clones = new List<GameObject>();
    #endregion

    BdTool bd;
    // 생성 버튼 클릭 후 건물 담는 변수
    Transform building;
    // 층 오브젝트 배열
    GameObject[] floor;
    // 층별 Y위치
    // tool obj이 포커싱(선택)이 벗어나면 리셋되버려서 static사용
    static float[] floorY;
    // 층별 on/off
    bool[] isOnOff;
    // 종류이름
    List<string> objType = new List<string>();
    // 모델이름
    List<string> objName = new List<string>();

    // 복사할 오브젝트 idx 3개 종류, 모델, 부모가 누군지..
    int[] selectedID = new int[3];


    //BdTool obj가 선택되면 한번 호출된다
    //다른데 클릭하고 다시 선택하면 또 호출됨
    private void OnEnable()
    {
        // target은 Monobehaviour속 transform처럼 Editor에 내장된 자료형 변수이다.
        // target은 [CustomEditor(typeof(BdObj))] 의 BdObj를 뜻하나 editor 자료형이라 변환해줘야 쓸 수 있음.
        // 이렇게 해줘서 이제 이 에디터가 bdobj 속 퍼블릭 변수에 접근이 가능하다.
        bd = (BdTool)target;
        //editor에서는 툴 오브젝트가 선택되지 않으면 저장된 모든 값이 초기화 되므로
        //다시 선택 할 때 변수가 어떤건지 잡아줘야 한다. 
        if (bd.building != null) building = bd.building;
        if (bd.floor != null) floor = bd.floor;
        if (bd.isOnOff != null) isOnOff = bd.isOnOff;

        //오브젝트 복제로 어떤종류인지 선택할 수 있게 세팅
        objType.Add("Walls");
        objType.Add("Furnitures");
        objType.Add("Products");
    }

    //BdTool obj가 선택된 상태로 마우스 커서가 씬에 있을 때 계속 호출된다.
    private void OnSceneGUI()
    {
        // 꼭 BdTool obj가 선택되어 있어야 작동하므로 
        // 다른데 클릭해도 선택 풀리지 않게하기(포커싱을 항상 bdtool로 한다. )
        int id = GUIUtility.GetControlID(FocusType.Passive);
        // 패시브로 세팅 후 디폴트로 추가해준다.
        HandleUtility.AddDefaultControl(id);

        //이건 빌딩 만드는 중에 도움되라고 선표시 만들기 
        //Handles.DrawLine(Vector3.zero, Vector3.one);
        Mouse();
    }

    void Mouse()
    {

        //레이 맞출 포지션 값(마우스) 가져오기 Vector2 형태임
        Event e = Event.current;

        //마우스 클릭하면 레이를 쏘자  >> 여기까지는 마우스 왼/오른쪽 구분 없이 눌러지는걸 뜻함
        if (e.type == EventType.MouseDown)
        {
            //에디터에서 레이 만들 때 쓰는 클래스가 따로 있음
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            // 항상 레이는 collider가 있어야 raycasthit이 발생한다. 잊지말자.
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                // 왼쪽버튼이면 >> int 값으로 가져옴 0 = 왼쪽 / 1 = 오른쪽
                if (e.button == 0)
                {
                    if (hit.transform.name == "Ground")
                        OnClickCreate(selectedID, hit.point, Vector3.zero, Vector3.one);
                    else
                    {
                        if (e.shift)
                        {
                            objInfo.RemoveAt(clones.IndexOf(hit.transform.gameObject));
                            clones.Remove(hit.transform.gameObject);
                            DestroyImmediate(hit.transform.gameObject);
                        }
                        else Selection.activeObject = hit.transform.gameObject;
                    }
                }
            }
        }
    }

    //BdTool cs 의 인스펙터 창에 커서가 올려져 있으면 호출된다
    public override void OnInspectorGUI()
    {
        //BdObj의 기본 기능이 유니티 상 보이게 해주는 역할 
        base.OnInspectorGUI();
        //or 위 베이스를 지운다면 아래껄 쓰면 됨
        // DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("Create"))
        {
            // print 함수(monobehaviour에만 있음)가 editor에는 없어서 확인하려면 debug.log 사용하기!
            // 플레이 안해도 작동됨!
            OnClickDelete();
            OnClickCreate();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Delete"))
        {
            OnClickDelete();
            floorY = null;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("1st On/Off"))
        {
            //빌딩이 있고
            if (building != null)
            {
                // 트랜스 폼.포지션.y를 바로 바꿀 수 없으니 그걸 담아줄 변수 선언
                Vector3 second = floor[2].transform.position;
                Vector3 third = floor[3].transform.position;

                isOnOff[1] = !isOnOff[1];
                // 1층 비활성화 or 활성화
                floor[1].SetActive(isOnOff[1]);

                //1층이 활성화면
                if (floor[1].activeSelf)
                {
                    //2층은 2층으로 3층도 3층자리지만 2층이 비활성화라면 2층자리로
                    second.y = floorY[2];
                    if (floor[2].activeSelf) third.y = floorY[3];
                    else third.y = floorY[2];
                }
                // 비활성화면
                else
                {
                    // 2층 포지션 값을 담은 변수 secend에 y값을 1층y로 바꿔준다. 
                    second.y = floorY[1];
                    // 3층도 2층 혹은 1층으로 바꿔준다
                    if (floor[2].activeSelf) third.y = floorY[2];
                    else third.y = floorY[1];
                }
                // 2, 3층 Y값 변경 (위에서 바꿔준 Y를 대입해준다.)
                floor[2].transform.position = second;
                floor[3].transform.position = third;
                bd.isOnOff[1] = isOnOff[1];
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("2nd On/Off"))
        {
            if (building != null)
            {
                Vector3 third = floor[3].transform.position;
                isOnOff[2] = !isOnOff[2];
                //2층 활성화 on/off
                floor[2].SetActive(isOnOff[2]);

                // 2층 활성화
                if (floor[2].activeSelf)
                {
                    // 1층 비활성화 >> 3층은 2층 높이
                    if (!floor[1].activeSelf) third.y = floorY[2];
                    // 아니면 3층
                    else third.y = floorY[3];
                }
                else
                {
                    //3층 Y값 바꾸기
                    // 1층도 비활성화 상태이면 y값은 1층
                    if (!floor[1].activeSelf) third.y = floorY[1];
                    // 아니면 2층
                    else third.y = floorY[2];
                }
                floor[3].transform.position = third;
                bd.isOnOff[2] = isOnOff[2];
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("3rd On/Off"))
        {
            // 3층 비활성화
            if (building != null)
            {
                isOnOff[3] = !isOnOff[3];
                floor[3].SetActive(isOnOff[3]);
                bd.isOnOff[3] = isOnOff[3];
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("All On/Off "))
        {

            if (building != null)
            {
                for (int i = 1; i < floor.Length; i++)
                {
                    //모두 On/Off 
                    isOnOff[i] = !isOnOff[i];
                    floor[i].SetActive(isOnOff[i]);
                    // 제자리 찾기
                    Vector3 pos = floor[i].transform.position;
                    pos.y = floorY[i];
                    floor[i].transform.position = pos;
                }
                bd.isOnOff = isOnOff;
            }
        }

        EditorGUILayout.Space();
        // 종류 목록 생성 및 선택적용
        selectedID[0] = EditorGUILayout.Popup("종류", selectedID[0], objType.ToArray());

        EditorGUILayout.Space();
        // 마우스 클릭하면 목록 변환하기
        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
            // 이전 목록 지우기
            if (objName.Count != 0) objName.Clear();
            // 종류 설정하기
            GameObject[] type = bd.walls;
            if (selectedID[0] == 1) type = bd.furnitures;
            if (selectedID[0] == 2) type = bd.products;

            //목록에 이름 넣기
            for (int i = 0; i < type.Length; i++)
            {
                objName.Add(type[i].name);
            }
        }

        // 배열의 개수가 안맞을 때 idx오류 발생함 >> 맨 마지막 번호로 바꿔준다.
        if (selectedID[1] > objName.Count - 1 || selectedID[1] < 0) selectedID[1] = objName.Count - 1;
        // 모델목록 생성 및 선택적용
        selectedID[1] = EditorGUILayout.Popup("모델", selectedID[1], objName.ToArray());

        EditorGUILayout.Space();
        if (GUILayout.Button("Import")) { ImportData(); }
        EditorGUILayout.Space();
        if (GUILayout.Button("Export")) { ExportData(); }




    }

    void OnClickCreate()
    {
        // PrefabUtility.InstantiatePrefab의 매개변수는 object인데 object는 GameObject와 같은거라 형변환만 해주면 됨.
        GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(bd.buildingA);
        //그리고 복제되는 obj 이름은 ~.name = "xx";으로 지정할 수 있다.
        a.name = "Building_base";

        building = a.transform;
        bd.building = building;

        //층
        floor = new GameObject[building.childCount];
        // 층별 Y값을 저장할 변수(x, z는 층마다 다를 수 있다.)
        floorY = new float[building.childCount];
        isOnOff = new bool[building.childCount];
        for (int i = 0; i < floor.Length; i++)
        {
            floor[i] = building.GetChild(i).gameObject;
            floorY[i] = building.GetChild(i).position.y;
            isOnOff[i] = true;
        }

        bd.floor = floor;
        bd.floorY = floorY;
        bd.isOnOff = isOnOff;
    }

    //idx는 selectedID / 0 == 종류, 1 == 오브젝트 2 == 부모
    void OnClickCreate(int[] idx, Vector3 pos, Vector3 rot, Vector3 scale)
    {

        GameObject[] obj = bd.walls;
        string type = "Walls";
        if (idx[0] == 1) { obj = bd.furnitures; type = "Furnitures"; }
        if (idx[0] == 2) { obj = bd.products; type = "Products"; }

        Transform parent = floor[idx[2]].transform;

        //전체 빌딩 찾기
        if (building != null)
        {

            GameObject a = (GameObject)PrefabUtility.InstantiatePrefab(obj[idx[1]]);

            if (idx[2] == 0)
            {
                // 1~3층 찾기 0번 자식은 table이라 1번부터 찾기
                for (int i = 1; i < building.childCount; i++)
                {
                    if (floor[i].activeSelf)
                    {
                        //해당 층에 귀속
                        parent = floor[i].transform;
                        idx[2] = i;
                        break;
                    }
                }
            }
            else { }

            ObjInfo info = new ObjInfo();
            // 데이터 및 복제 프리펩 값세팅
            a.transform.SetParent(parent);
            info.scale = a.transform.localScale = scale;
            info.pos = a.transform.localPosition = pos;
            info.rot = a.transform.localEulerAngles = rot;
            for (int i = 0; i < idx.Length; i++)
            {
                info.objidx[i] = idx[i];
            }

            objInfo.Add(info);
            clones.Add(a);
            Debug.Log(" info : " + type + " " + a.name + " " + info.objidx[2] + "층");
            idx[2] = 0;
        }

    }

    void OnClickCreate(ObjInfo info)
    {
        OnClickCreate(info.objidx, info.pos, info.rot, info.scale);
    }

    void OnClickDelete()
    {
        GameObject a = GameObject.Find("Building_base");
        //에디터 모드에서만 아래 코드 사용하기!
        //일반 모드에서 잘못 사용하다간 에셋의 중요한 부분을 삭제해 큰 문제를 야기함.
        DestroyImmediate(a);
        //빌딩 지우면 정보도 지워지기
        objInfo.Clear();
        clones.Clear();
        building = bd.building = null;
        floor = bd.floor = null;
        floorY = bd.floorY = null;
        isOnOff = bd.isOnOff = null;
    }

    void ExportData()
    {
        for (int i = 0; i < objInfo.Count; i++)
        {
            objInfo[i].pos = clones[i].transform.localPosition;
            objInfo[i].rot = clones[i].transform.localEulerAngles;
            objInfo[i].scale = clones[i].transform.localScale;
        }
        // UserData> Data > ObjInfo 순
        ObjectData obj = new ObjectData();
        // ObjectData  형식을 제이슨을 통해 스트링으로변환
        obj.info = objInfo;
        string json = JsonUtility.ToJson(obj, true);
        Debug.Log(json);

        // 컴퓨터에 빈 텍스트 파일 생성 
        FileStream file = new FileStream(Application.streamingAssetsPath +"Building_data.json", FileMode.Create);
        // 제이슨 데이터를 텍스트로 전환
        byte[] byteData = Encoding.UTF8.GetBytes(json);
        // 파일 덮어쓰기
        file.Write(byteData, 0, byteData.Length);
        // 닫아주기!!!
        file.Close();

    }

    void ImportData()
    {
        string path = Application.streamingAssetsPath + "Building_data.json";
        //파일 있니?
        if (!File.Exists(path)) return;

        OnClickDelete();
        OnClickCreate();
        // 열기
        FileStream file = new FileStream(path, FileMode.Open);
        //저장된 데이터 byte로 담기(불러오기 = 읽어오기)
        byte[] byteData = new byte[file.Length];
        file.Read(byteData, 0, byteData.Length);
        //다시 닫아주기!
        file.Close();

        //텍스트 파일이므로 byte를 스트링 변환해주기
        string txt = Encoding.UTF8.GetString(byteData);
        //다시 제이슨을 통해  ObjectData 형식으로 변환시킨다.
        ObjectData obj = JsonUtility.FromJson<ObjectData>(txt);
        for (int i = 0; i < obj.info.Count; i++)
        {
            OnClickCreate(obj.info[i]);
        }
    }

    //유저1번 2번 3번.. 으로 데이터 추가해 파일 뽑으려 만든 함수.. 수정필요
    void FinalData(ObjectData data)
    {
        // 매개변수 다시 설정하기
        objdata.Add(data);
        UserData obj = new UserData();
        obj.data = objdata;
        obj.userID = "User_no." + objdata.Count;
        Debug.Log("빌딩 정보 갯수 : " + objdata.Count);

        string json = JsonUtility.ToJson(obj, true);
        Debug.Log(json);
    }

    // 유저123.... 모든 유저 정보데이터 없에주기
    void AllDataReset()
    {
        objdata.Clear();
        OnClickDelete();
    }


}


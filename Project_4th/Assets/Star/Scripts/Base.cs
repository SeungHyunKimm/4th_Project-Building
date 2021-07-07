using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

[Serializable]
public class ObjInfo2
{
    //타입 / 모델 / 층
    public int[] objidx = new int[3];
    public Vector3 pos; // localposition
    public Vector3 rot; // eularangle 사용할 것
    public Vector3 scale;
}

[Serializable]
public class ObjectData2
{
    public List<ObjInfo2> info = new List<ObjInfo2>();
}
public class Base : MonoBehaviour
{
    List<ObjInfo2> objInfo = new List<ObjInfo2>();
    List<GameObject> clones = new List<GameObject>();

    GameObject[] floor;
    float[] floorY;
   public bool[] isOnOff;

    public GameObject[] walls;
    public GameObject[] furnitures;
    public GameObject[] products;



    void Start()
    {
        floor = new GameObject[transform.childCount];
        // 층별 Y값을 저장할 변수(x, z는 층마다 다를 수 있다.)
        floorY = new float[transform.childCount];
        isOnOff = new bool[transform.childCount];
        for (int i = 0; i < floor.Length; i++)
        {
            floor[i] = transform.GetChild(i).gameObject;
            floorY[i] = transform.GetChild(i).position.y;
            isOnOff[i] = true;
        }
    }

 
    public void OnClickImportData() {

        string path = Application.dataPath + "/Star/Editor/Building_data.json";
        //파일 있니?
        if (!File.Exists(path)) return;

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
        ObjectData2 obj = JsonUtility.FromJson<ObjectData2>(txt);
        for (int i = 0; i < obj.info.Count; i++)
        {
            OnClickCreate(obj.info[i]);
        }
    }


    public void OnClickExportData() {
        for (int i = 0; i < objInfo.Count; i++)
        {
            objInfo[i].pos = clones[i].transform.localPosition;
            objInfo[i].rot = clones[i].transform.localEulerAngles;
            objInfo[i].scale = clones[i].transform.localScale;
        }
        // UserData> Data > ObjInfo 순
        ObjectData2 obj = new ObjectData2();
        // ObjectData  형식을 제이슨을 통해 스트링으로변환
        obj.info = objInfo;
        string json = JsonUtility.ToJson(obj, true);
        Debug.Log(json);

        // 컴퓨터에 빈 텍스트 파일 생성 
        FileStream file = new FileStream(Application.dataPath + "/Star/Editor/Building_data_holo.json", FileMode.Create);
        // 제이슨 데이터를 텍스트로 전환
        byte[] byteData = Encoding.UTF8.GetBytes(json);
        // 파일 덮어쓰기
        file.Write(byteData, 0, byteData.Length);
        // 닫아주기!!!
        file.Close();
    }


    public void OnClickCreate(int[] idx, Vector3 pos, Vector3 rot, Vector3 scale)
    {

        GameObject[] obj = walls;
        string type = "Walls";
        if (idx[0] == 1) { obj = furnitures; type = "Furnitures"; }
        if (idx[0] == 2) { obj = products; type = "Products"; }

        Transform parent = floor[idx[2]].transform;
    
        GameObject a = Instantiate(obj[idx[1]]);

        if (idx[2] == 0)
        {
            // 1~3층 찾기 0번 자식은 table이라 1번부터 찾기
            for (int i = 1; i < transform.childCount; i++)
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

          a.transform.SetParent(parent);
         ObjInfo2 info = new ObjInfo2();
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
        

      void OnClickCreate(ObjInfo2 info)
    {
        OnClickCreate(info.objidx, info.pos, info.rot, info.scale);
    }  

    // 아이템 없에는 함수
    // 내가 어떤걸 집었는지 알 수 없어 매개변수로 담게 만듬
    public void OnClickDestroy(GameObject a)
    {
        // 레이로 집어서 쓰레기통에 버린다. How?? 고민하자
        objInfo.RemoveAt(clones.IndexOf(a));
        clones.Remove(a);
        a.SetActive(false);
       //Destroy(a);
    }

    public void OnClickDelete() {
        for (int i = 0; i < clones.Count; i++)
        {
            Destroy(clones[i]);
        }
        objInfo.Clear();
        clones.Clear();
    }

    //층 오브젝트 자식 중 XYZ레이어면 objmani_star.cs에서 Scale_x로 바꾼다.. 
    public void OnClickScaleXTotal()
    {
        for (int i = 0; i < floor.Length; i++)
        {
            //~층이 활성화인 경우
            if (floor[i].activeSelf)
            {
                // 자식 수만큼 실행
                for (int j = 0; j < floor[i].transform.childCount; j++)
                {
                    //자식의 레이어가 xyz면
                    GameObject child = floor[i].transform.GetChild(j).gameObject;
                    if (child.layer == LayerMask.NameToLayer("XYZ"))
                    {
                        //scale_x로 바꾼다.
                        ObjectManipulator_Star objstar = child.GetComponent<ObjectManipulator_Star>();
                        objstar.OnClickScaleX();
                    }


                }

            }

        }

    }


    public void OnClickScaleYTotal()
    {
        for (int i = 0; i < floor.Length; i++)
        {
            //~층이 활성화인 경우
            if (floor[i].activeSelf)
            {
                // 자식 수만큼 실행
                for (int j = 0; j < floor[i].transform.childCount; j++)
                {
                    //자식의 레이어가 xyz면
                    GameObject child = floor[i].transform.GetChild(j).gameObject;
                    if (child.layer == LayerMask.NameToLayer("XYZ"))
                    {
                        //scale_x로 바꾼다.
                        ObjectManipulator_Star objstar = child.GetComponent<ObjectManipulator_Star>();
                        objstar.OnClickScaleY();
                    }


                }

            }

        }

    }


    public void OnClickScaleZTotal()
    {
        for (int i = 0; i < floor.Length; i++)
        {
            //~층이 활성화인 경우
            if (floor[i].activeSelf)
            {
                // 자식 수만큼 실행
                for (int j = 0; j < floor[i].transform.childCount; j++)
                {
                    //자식의 레이어가 xyz면
                    GameObject child = floor[i].transform.GetChild(j).gameObject;
                    if (child.layer == LayerMask.NameToLayer("XYZ"))
                    {
                        //scale_x로 바꾼다.
                        ObjectManipulator_Star objstar = child.GetComponent<ObjectManipulator_Star>();
                        objstar.OnClickScaleZ();
                    }


                }

            }

        }

    }

    //미니어쳐 함수
    //송도 작은 구역과 함께 나타나게 수정하기
    public void OnClickMini(Transform player) {
        
       // 건물 본체 복사
        Transform mini = Instantiate(gameObject).transform;
        mini.name = "Mini";
        mini.GetComponent<Base>().enabled = false ;
        // 바운드 컨트롤로 움직이게 하자
        mini.GetComponent<BoundsControl>().enabled = true;
        mini.GetComponent<ObjectManipulator>().enabled = true;
        // 미니어쳐라 10분의 1크기
        mini.localScale = Vector3.one * .1f;
        // 포톤 접속 시 플레이어 찾을 수 없어 매개변수로 받아 실행하자.
        // 플레이어 스크립트에서 base.cs 받아와서 버튼으로 처리하기
        mini.position = player.position + Vector3.forward * .1f;

    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Photon.Pun;
using Photon.Realtime;
using Microsoft.MixedReality.Toolkit.Input;
using MRTK.Tutorials.MultiUserCapabilities;


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

public class Base : MonoBehaviourPunCallbacks
{
    List<ObjInfo2> objInfo = new List<ObjInfo2>();
   public List<GameObject> clones = new List<GameObject>();

    GameObject[] floor;
    float[] floorY;
   public bool[] isOnOff;

    public GameObject[] walls;
    public GameObject[] furnitures;
    public GameObject[] products;

    PhotonView pv;

    GameObject tem;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        //포톤 뷰 붙는 프리펩들 포톤/리소스 폴더 안에 없어도 문제없게 하기
        if (PhotonNetwork.PrefabPool is DefaultPool pool)
        {
            for (int i = 0; i < walls.Length; i++)
            {
            if (walls[i] != null) pool.ResourceCache.Add(walls[i].name, walls[i]);
            }  
            
            for (int i = 0; i < furnitures.Length; i++)
            {
            if (furnitures[i] != null) pool.ResourceCache.Add(furnitures[i].name, furnitures[i]);
            }  
            
            for (int i = 0; i < products.Length; i++)
            {
            if (products[i] != null) pool.ResourceCache.Add(products[i].name, products[i]);
            }

        }

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
        OnClickDelete();
        string path = Application.streamingAssetsPath + "/Building_data.json";
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
        FileStream file = new FileStream(Application.streamingAssetsPath + "/Building_data_runtime.json", FileMode.Create);
        // 제이슨 데이터를 텍스트로 전환
        byte[] byteData = Encoding.UTF8.GetBytes(json);
        // 파일 덮어쓰기
        file.Write(byteData, 0, byteData.Length);
        // 닫아주기!!!
        file.Close();
    }


    public void OnClickCreate(int[] idx, Vector3 pos, Vector3 rot, Vector3 scale)
    {
       // if (!PhotonNetwork.IsMasterClient) return;

        GameObject[] obj = walls;
        if (idx[0] == 1) { obj = furnitures;  }
        if (idx[0] == 2) { obj = products; }

        //GameObject a = Instantiate(obj[idx[1]]);
        //포톤뷰가 붙어있어 그냥 복제 불가능, 포톤네트워크 통해 복제하되 위치, 회전, 크기는 아래에서 조절할 것이므로 아무 값이나 넣자.
        tem = PhotonNetwork.Instantiate(obj[idx[1]].name, Vector3.one * 1000, Quaternion.identity) ;
        pv.RPC("RPCObjDataAdd", RpcTarget.All, idx.Length, idx[0], idx[1], idx[2], pos, rot, scale);

            }

    [PunRPC]
    void RPCObjDataAdd(int length, int idx0, int idx1, int idx2, Vector3 pos, Vector3 rot, Vector3 scale) {

        Transform parent = floor[idx2].transform;
        int[] idx = { idx0, idx1, idx2 };

        if (idx2 == 0)
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

        tem.transform.SetParent(parent);
        ObjInfo2 info = new ObjInfo2();
        info.scale = tem.transform.localScale = scale;
        info.pos = tem.transform.localPosition = pos;
        info.rot = tem.transform.localEulerAngles = rot;

        for (int i = 0; i < length; i++)
        {
            info.objidx[i] = idx[i];
        }

        objInfo.Add(info);
        clones.Add(tem);
        Debug.Log(" info : " + tem.name + " " + info.objidx[2] + "층");
        idx[2] = 0;
        tem = null;

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

        //a.transform.position = Vector3.one * 1000;
        //a.SetActive(false);
        //디스트로이하면 mrtk자체 오류남
        //Destroy(a);

        PhotonNetwork.Destroy(a);
    }

    //데이터 없이 삭제만 한다.
    public void OnClickDestroy(Collider other)
    {
        PhotonNetwork.Destroy(other.gameObject);
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
    //그냥 베이스를 콘타의 자식으로 우선 해두고 콘타 메쉬를 비활성화 하자..?
    //** objstar랑 nearGrab, pv(takeover), ownershiptransfer, netsyc_star 추가되고
    // 메쉬 콜라이더 있는 콘타프리펩 퍼블릭으로 가져오기(convex!)
    //** 콘타랑 베이스 프리펩풀에 넣기 베이스 이름은 미니로 변경
    //**포톤으로 복제하기
    //미니를 콘타의 자식으로 바꾸기
    //미니 위치 조금 위로 만들기

    public void OnClickMini(Transform player) {

        if (PhotonNetwork.PrefabPool is DefaultPool pool) {
            if (pool.ResourceCache.ContainsKey("Mini"))
            {
                pool.ResourceCache.Remove("Mini");
               pool.ResourceCache.Add("Mini", gameObject);
            }
            else { 
               pool.ResourceCache.Add("Mini", gameObject);
            }
        }

       // 건물 본체 복사
        Transform mini = PhotonNetwork.Instantiate("Mini", Vector3.one * 1000, Quaternion.identity).transform;
       
        //포톤뷰로 그냥 복사가 불가능해 변경
        // Transform mini = Instantiate(gameObject).transform;
        //mini.name = "Mini";
        mini.GetComponent<Base>().enabled = false ;
        // 잡고 조정 가능하게 하자
        mini.GetComponent<BoundsControl>().enabled = true;
        mini.GetComponent<ObjectManipulator>().enabled = true;
        mini.GetComponent<NearInteractionGrabbable>().enabled = true;
        // 미니어쳐라 10cm
        mini.localScale = Vector3.one * .0125f;
        // 포톤 접속 시 플레이어 찾을 수 없어 매개변수로 받아 실행하자.
        mini.position = player.position + Vector3.forward * .1f;

    }

    public void GetShared(Transform parent)
    {
        // 마스터면 리턴하기 두번째 유저만 앵커 가져와야 함
        if (PhotonNetwork.IsMasterClient) return;
        SharingModuleScript sms = parent.GetComponent<SharingModuleScript>();
        sms.GetAzureAnchor();
    }
}

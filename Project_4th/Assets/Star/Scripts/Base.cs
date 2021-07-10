using System.Collections;
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
    #region 변수들
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
    int listidx;
    Transform mini;
    public GameObject conta;
    #endregion

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

    #region 데이터 불러오기 / 내보내기
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
            objInfo[i].pos = clones[i].transform.position;
            objInfo[i].rot = clones[i].transform.eulerAngles;
            objInfo[i].scale = clones[i].transform.localScale;
        }
        // UserData> Data > ObjInfo 순
        ObjectData2 obj = new ObjectData2();
        // ObjectData  형식을 제이슨을 통해 스트링으로변환
        obj.info = objInfo;
        string json = JsonUtility.ToJson(obj);

        // 컴퓨터에 빈 텍스트 파일 생성
        FileStream file = new FileStream(Application.streamingAssetsPath + "/Building_data.json", FileMode.Create);
        // 제이슨 데이터를 텍스트로 전환
        byte[] byteData = Encoding.UTF8.GetBytes(json);
        // 파일 덮어쓰기
        file.Write(byteData, 0, byteData.Length);
        // 닫아주기!!!
        file.Close();
    }
    #endregion 

    #region 빌딩 아이템 생성
    public void OnClickCreate(int[] idx, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        // if (!PhotonNetwork.IsMasterClient) return;

        GameObject[] obj = walls;
        if (idx[0] == 1) { obj = furnitures; }
        if (idx[0] == 2) { obj = products; }

        //GameObject a = Instantiate(obj[idx[1]]);
       // 포톤으로 삭제 시 마스터가 생성한게 아니라고 안삭제된다. 마스터만 생성하게 하자
        pv.RPC("RPCMasterCreate", RpcTarget.MasterClient, obj[idx[1]].name);
        pv.RPC("RPCObjDataAdd", RpcTarget.All, idx, pos, rot, scale, obj[idx[1]].name);
    }

    [PunRPC]
    void RPCMasterCreate(string name) {
        //포톤뷰가 붙어있어 그냥 복제 불가능, 포톤네트워크 통해 복제하되 위치, 회전, 크기는 아래에서 조절할 것이므로 아무 값이나 넣자.
        tem = PhotonNetwork.Instantiate(name, Vector3.one * 1000, Quaternion.identity);
    }
    [PunRPC]
    void RPCObjDataAdd(int[] idx, Vector3 pos, Vector3 rot, Vector3 scale, string name) {

        Transform parent = floor[idx[2]].transform;

        if (tem == null)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Items");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].name.Contains(name))
                {
                    //print(temp[i].transform.parent);
                    if (temp[i].transform.parent == null)
                    {
                        tem = temp[i];
                        break;
                    }
                }

            }
        }

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

        ObjInfo2 info = new ObjInfo2();
        tem.transform.SetParent(parent);
        info.scale = tem.transform.localScale = scale;
        info.pos = tem.transform.position = pos
            //+ new Vector3(0, .3f, 0)
            ;
        info.rot = tem.transform.eulerAngles = rot;

        for (int i = 0; i < idx.Length; i++)
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
    #endregion

    #region 삭제
    // 아이템 없에는 함수
    // 내가 어떤걸 집었는지 알 수 없어 매개변수로 담게 만듬
    public void OnClickDestroy(GameObject a)
    {
        // 레이로 집어서 쓰레기통에 버린다. How?? 고민하자
        listidx = clones.IndexOf(a);
        pv.RPC("RPCDataRemove", RpcTarget.All, listidx);
        //a.transform.position = Vector3.one * 1000;
        //Destroy(a);
        //PhotonNetwork.Destroy(a);
    }

    // 데이터 부분삭제 동기화
    [PunRPC]
    void RPCDataRemove(int list) {

        GameObject a = clones[list];
        //디스트로이하면 mrtk자체 오류남
        a.transform.position = Vector3.one * 1000;
        a.SetActive(false);
        objInfo.RemoveAt(list);
        clones.RemoveAt(list);
    }

    //데이터 없이 삭제만 한다. >> 삭제 오류로 비활성화
    public void OnClickDestroy(Collider other)
    {
        pv.RPC("RPCItemInactive", RpcTarget.All, other.gameObject.name);
    }

    [PunRPC]
    void RPCItemInactive(string name) {
        GameObject a = GameObject.Find(name);
        a.transform.position = Vector3.one * 1000;
        a.SetActive(false);
    }

    public void OnClickDelete()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < clones.Count; i++)
            {
                PhotonNetwork.Destroy(clones[i]);
            //clones[i].SetActive(false);
            }
        }
        else 
        { 
          pv.RPC("RPCMasterDestroy", RpcTarget.MasterClient);
        }
        //데이터 삭제 동기화
        pv.RPC("RPCClear", RpcTarget.All);
    }

    [PunRPC]
    void RPCClear()
    {
        objInfo.Clear();
        clones.Clear();
    }

    [PunRPC]
    void RPCMasterDestroy() {
    
            for (int i = 0; i < clones.Count; i++)
            {
                PhotonNetwork.Destroy(clones[i]);
            }
            
    }
    #endregion

    #region 스케일 조절
    //층 오브젝트 자식 중 XYZ레이어면 objmani_star.cs에서 Scale_x로 바꾼다.. 
    public void OnClickScaleXTotal()
    {
        pv.RPC("RPC_X", RpcTarget.All);
    }

    [PunRPC]
    void RPC_X() {
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
        pv.RPC("RPC_Y", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Y()
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
        pv.RPC("RPC_Z", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Z()
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
    #endregion

    #region    //미니어쳐 함수
    //송도 작은 구역과 함께 나타나게 수정하기
    // 초반에는 베이스를 콘타의 자식으로 두고 콘타도 포톤프리펩화하려했으나
    // 그냥 별개로 생성 후 특정 포인트를 부딪히면 콘타가 미니어쳐 빌딩의 자식이 되게 코드 짬.
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
        mini = PhotonNetwork.Instantiate("Mini", Vector3.one * 1000, Quaternion.identity).transform;
        //포톤뷰로 그냥 복사가 불가능해 변경
        // Transform mini = Instantiate(gameObject).transform;
        pv.RPC("RPCMini", RpcTarget.All, player.position);
    }

    [PunRPC]
    void RPCMini(Vector3 pos) {

        if (mini == null) {
            mini = GameObject.Find("Base(Clone)(Clone)").transform;
        }
        mini.name = "Mini";
    
        mini.GetComponent<Base>().enabled = false;
        // 잡고 조정 가능하게 하자
        mini.GetComponent<BoundsControl>().enabled = true;
        mini.GetComponent<ObjectManipulator>().enabled = true;
        mini.GetComponent<NearInteractionGrabbable>().enabled = true;
        mini.gameObject.AddComponent<Attach>();

        // 미니어쳐라 10cm
        mini.localScale = Vector3.one * .0125f;
        // 포톤 접속 시 플레이어 찾을 수 없어 매개변수로 받아 실행하자.
        mini.position = pos + Vector3.forward * .1f;
        Transform a = Instantiate(conta).transform;
        a.localScale = Vector3.one * .0125f;
        a.position = new Vector3(mini.position.x, mini.position.y - .5f , mini.position.x);
        mini = null;
    }
    #endregion

    //공간인식용
    public void GetShared(Transform parent)
    {
        SharingModuleScript sms = parent.GetComponent<SharingModuleScript>();
        // 마스터면 리턴하기 다른 유저만 앵커 가져와야 함
        if (PhotonNetwork.IsMasterClient) return;
       // AnchorModuleScript ams = parent.GetComponent<AnchorModuleScript>();
        sms.GetAzureAnchor();
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
//using Photon.Realtime;


public class NetManager : MonoBehaviour
//PunCallbacks
{
    string ver = "1";
    //PhotonView pv;
    int objID;

    void Start()
    {
        //pv = GetComponent<PhotonView>();
        //PhotonNetwork.GameVersion = ver;
        
    }

    //public override void OnConnected()
    //{
    //    base.OnConnected();
    //    print("OnConnected");
    //}

    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
    //    PhotonNetwork.JoinLobby();
    //}

    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print("로비접속");
    //    RoomOptions option = new RoomOptions();
    //     option.MaxPlayers = 2;

    //건축주(고객)의 초기 빌딩 오브젝트의 ID를 포톤으로 불러오기 위한 Hashtable 설정
    // 룸옵션즈가 만들어진 함수에 같이 있어야 함
    //ExitGames.Client.Photon.Hashtable custom = new ExitGames.Client.Photon.Hashtable();
    // custom["obj_type"] = 1;
    // option.CustomRoomProperties = custom;
    //  string[] keys = { "obj_type" };
    //option.CustomRoomPropertiesForLobby = keys;

    //    PhotonNetwork.JoinOrCreateRoom("AA", option, TypedLobby.Default);
    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    print("방만듬");
    //}

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print("방접속");
    //    BuildingData.instance.objID = (int)PhotonNetwork.CurrentRoom.CustomProperties["obj_type"];
    //    PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
    //}


    //방나가기
    //public void LeaveRoom()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    PhotonNetwork.Disconnect();
    //    print("방나감");
    //}

    //플레이어 들어왔을 때 체크
    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);
    //    print("신규접속자 : "+ newPlayer);
    //}



    //플레이어 나갔을 때 체크
    //    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //    {
    //        base.OnPlayerLeftRoom(otherPlayer);
    //print("퇴장 : "+ newPlayer);
    //    }
}
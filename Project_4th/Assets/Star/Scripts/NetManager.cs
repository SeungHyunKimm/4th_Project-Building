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
    //    print("�κ�����");
    //    RoomOptions option = new RoomOptions();
    //     option.MaxPlayers = 2;

    //������(��)�� �ʱ� ���� ������Ʈ�� ID�� �������� �ҷ����� ���� Hashtable ����
    // ��ɼ�� ������� �Լ��� ���� �־�� ��
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
    //    print("�游��");
    //}

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    print("������");
    //    BuildingData.instance.objID = (int)PhotonNetwork.CurrentRoom.CustomProperties["obj_type"];
    //    PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
    //}


    //�泪����
    //public void LeaveRoom()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    PhotonNetwork.Disconnect();
    //    print("�泪��");
    //}

    //�÷��̾� ������ �� üũ
    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);
    //    print("�ű������� : "+ newPlayer);
    //}



    //�÷��̾� ������ �� üũ
    //    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //    {
    //        base.OnPlayerLeftRoom(otherPlayer);
    //print("���� : "+ newPlayer);
    //    }
}
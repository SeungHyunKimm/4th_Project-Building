using Photon.Pun;
using UnityEngine;
using System.Collections;


namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonUser : MonoBehaviour
    {
        public PhotonView pv;
        private string username;
        int set = 0;

        private void Start()
        {
            pv = GetComponent<PhotonView>();
           
            // 포톤뷰가 내께 아니면 리턴
            if (!pv.IsMine) return;
           username = "User" + PhotonNetwork.NickName;
           pv.RPC("PunRPC_SetNickName", RpcTarget.AllBuffered, username);

            StartCoroutine(ASA());
        }


        [PunRPC]
        private void PunRPC_SetNickName(string nName)
        {
            gameObject.name = nName;
        }

        [PunRPC]
        private void PunRPC_ShareAzureAnchorId(string anchorId)
        {
            GenericNetworkManager.Instance.azureAnchorId = anchorId;

            Debug.Log("\nPhotonUser.PunRPC_ShareAzureAnchorId()");
            Debug.Log("GenericNetworkManager.instance.azureAnchorId: " + GenericNetworkManager.Instance.azureAnchorId);
            Debug.Log("Azure Anchor ID shared by user: " + pv.Controller.UserId);
            set = 1;
        }

        public void ShareAzureAnchorId()
        {
            if (pv != null)
                pv.RPC("PunRPC_ShareAzureAnchorId", RpcTarget.AllBuffered,
                    GenericNetworkManager.Instance.azureAnchorId);
            else
                Debug.LogError("PV is null");
        }

        IEnumerator ASA() {
           
            //혼자 접속 중이면 while 돌기
            while (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
            yield return null;
            }

            GameObject anchor = GameObject.Find("Photon_Location");
            AnchorModuleScript ams = anchor.GetComponent<AnchorModuleScript>();
            SharingModuleScript sms = anchor.GetComponent<SharingModuleScript>();

            if (PhotonNetwork.IsMasterClient)
            {
                // 둘 이상이면 
                ams.StartAzureSession();
                // 유저의 부모는 포톤로케이션(테이블앵커 붙어있음)이지만 바로 생성될 때는 아니다..;
                // 세션 시작하고 정보받는 대기 시간 있음.. bool만들어 코루틴 넣음.. start 시 true로 create 시 false로 바뀜
                while (ams.isdone == false)
                {
                    yield return null;
                }
                ams.CreateAzureAnchor(anchor);
                // 이것도 대기시간 있음
                while (ams.isdone == true)
                {
                    yield return null;
                }
                //앵커모듈 쉐어 rpc로 쏴주는게 있어서 대기시간 만듬
                sms.ShareAzureAnchor();
                // 다른 접속자도 동일하게 대기해야 되서 인트값 변경으로 넣어줌
                while (set == 0)
                {
                    yield return null;
                }
                pv.RPC("RPCAllSet", RpcTarget.OthersBuffered);
            }
            //else {

            //    while (set == 0)
            //    {
            //        yield return null;
            //    }
            //    ams.StartAzureSession();
            //}
           
        }

        [PunRPC]
        void RPCAllSet() {
            GameObject anchor = GameObject.Find("Photon_Location");
            AnchorModuleScript ams = anchor.GetComponent<AnchorModuleScript>();
            ams.StartAzureSession();
        }
    }
}

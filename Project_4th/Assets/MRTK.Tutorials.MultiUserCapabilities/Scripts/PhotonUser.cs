using Photon.Pun;
using UnityEngine;
using System.Collections;


namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonUser : MonoBehaviour
    {
        public PhotonView pv;
        private string username;

        private void Start()
        {
            pv = GetComponent<PhotonView>();
           
            // 포톤뷰가 내께 아니면 리턴
            if (!pv.IsMine) return;
           username = "User" + PhotonNetwork.NickName;
           pv.RPC("PunRPC_SetNickName", RpcTarget.AllBuffered, username);

           // StartCoroutine(ASA());
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
            while (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
            {
            yield return null;
            }

            // 둘 이상이면 
            AnchorModuleScript ams = transform.parent.GetComponent<AnchorModuleScript>();
            //앵커모듈 동기화하기
            ams.StartAzureSession();
            // 내가 마스터가 아니면 빠져나오기
            if (!PhotonNetwork.IsMasterClient) yield break;
            // 유저의 부모는 포톤로케이션(테이블앵커 붙어있음);
            ams.CreateAzureAnchor(transform.parent.gameObject);
            SharingModuleScript sms = transform.parent.GetComponent<SharingModuleScript>();
            sms.ShareAzureAnchor();

        }
    }
}

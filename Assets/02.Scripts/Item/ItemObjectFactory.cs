using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{
    private PhotonView _photonView;
   public static ItemObjectFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RequestCreate(ItemType type, Vector3 position)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Create(type, position);
        }
        else
        {
            _photonView.RPC(nameof(Create), RpcTarget.MasterClient, type, position);
        }
    }

    [PunRPC]
    private void Create(ItemType type, Vector3 position)
    {
        Vector3 dropPos = position + new Vector3(0.5f, 0.5f, 0f) + UnityEngine.Random.insideUnitSphere;
        PhotonNetwork.InstantiateRoomObject(type.ToString(), dropPos, Quaternion.identity);      
    }

    public void RequestDelete(int viewID)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Delete(viewID);
        }
        else
        {
            photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewID);
        }
    }


    [PunRPC]
    private void Delete(int viewID)
    {
        // photonview.find(viewID)로 게임 오브젝트를 검색해오는 기능
        GameObject objectToDelete = PhotonView.Find(viewID).gameObject; 
        if(objectToDelete != null)
        {
            PhotonNetwork.Destroy(objectToDelete);
        }
    }
}

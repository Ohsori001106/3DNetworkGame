using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAttackAbility))]
[RequireComponent(typeof(CharacterMoveAbility))]
[RequireComponent(typeof(CharacterRotateAbility))]

public class Character : MonoBehaviour, IPunObservable
{
    public Stat Stat;
    public PhotonView PhotonView { get; private set; }
    private void Awake()
    {
        Stat.Init();
        PhotonView =GetComponent<PhotonView>();
        if(PhotonView.IsMine)
        {
            UI_CharacterStat.Instance.MyCharacter = this;
        }
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // stream(통로)은 서버에서 주고받을 데이터가 담겨있는 변수
        if (stream.IsWriting) // 데이터를 전송하는 상황
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading) // 데이터를 수신하는 상황
        {
            Vector3 receivedPosition = (Vector3)stream.ReceiveNext();
            Quaternion receivedRotation = (Quaternion)stream.ReceiveNext();

            if(!PhotonView.IsMine)
            {
                transform.position = receivedPosition;
                transform.rotation = receivedRotation;
            }
            
                        
        }
        // info는 송수신 성공/실패 여부에 대한 메시지가 담겨있다.
    }

}

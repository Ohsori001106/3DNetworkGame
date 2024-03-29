using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaharacterMotionAbility : CharacterAbility
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Owner.State == State.Death || !Owner.PhotonView.IsMine)
            {
                return;
            }
            Owner.PhotonView.RPC(nameof(PlayMotion), Photon.Pun.RpcTarget.All, 1);
        }
    }
    
    private void PlayMotion(int number)
    {
        GetComponent<Animator>().SetTrigger($"Motion{number}");
    }
}

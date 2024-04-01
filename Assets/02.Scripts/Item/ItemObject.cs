using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Collider))]
public class ItemObject : MonoBehaviour
{
    [Header("아이템 타입")]
    public ItemType ItemType;

    public float Value = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.GetComponent<Character>();
            if (character.State == State.Death)
            {
                return;
            }

            switch (ItemType)
            {
                case ItemType.Health_potion:
                {
                    character.Stat.Health += Value;
                    break;
                }
                case ItemType.Stamina_potion:
                {
                    character.Stat.Stamina += Value;
                    break;
                }
            }
        }
            

        
    }
}

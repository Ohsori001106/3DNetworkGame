using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAttackAbility))]
[RequireComponent(typeof(CharacterMoveAbility))]
[RequireComponent(typeof(CharacterRotateAbility))]

public class Character : MonoBehaviour
{
    public Stat Stat;

    private void Start()
    {
        Stat.Init();
    }
}

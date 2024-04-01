using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public float   Health;
    public float   MaxHealth;
    public float MoveSpeed;
    public float RunSpeed;
    public float RotationSpeed;

    public float AttackCoolTime;
    public float AttackConsumeStamina = 10;

    public float Stamina;
    public float MaxStamina;
    public float RecoveryStamina = 5;
    public float RunConsumeStamina = 10;

    public float JumpPower;
    public float JumpConsumeStamina = 10;

    public bool IsJumping;

    public int Damage;
    public void Init()
    {
        Health = MaxHealth;
        Stamina = MaxStamina;
    }

    
}

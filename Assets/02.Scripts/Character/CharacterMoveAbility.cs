using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMoveAbility : CharacterAbility
{
    private CharacterController _characterController;
    private Animator _animator;

    


    private bool _isRunning;
    [SerializeField]
    private float gravity = -9.81f; // 중력 값 설정

    private void Start()
    {
       
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Owner.State == State.Death || !Owner.PhotonView.IsMine)
        {
            return;
        }
        float speed = Owner.Stat.MoveSpeed;
        _isRunning = false;
        if (Input.GetKey(KeyCode.LeftShift) && Owner.Stat.Stamina > 0)
        {
            _isRunning = true;
            speed = Owner.Stat.RunSpeed;
            Owner.Stat.Stamina -= Owner.Stat.RunConsumeStamina * Time.deltaTime;
        }
        else
        {
            Owner.Stat.Stamina += Owner.Stat.RecoveryStamina * Time.deltaTime;
            if(Owner.Stat.Stamina >= Owner.Stat.MaxStamina)
            {
                Owner.Stat.Stamina = Owner.Stat.MaxStamina;
            }
        }
        

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        _animator.SetFloat("Move", dir.magnitude);

        if (!_characterController.isGrounded)
        {
            dir.y += gravity * Time.deltaTime; // 중력을 적용합니다.
        }

        _characterController.Move(dir * (speed * Time.deltaTime));
    }

    public void Teleport(Vector3 position)
    {
        _characterController.enabled = false;

        transform.position = position;

        _characterController.enabled = true;
    }
}

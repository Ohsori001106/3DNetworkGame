using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMoveAbility : CharacterAbility
{
    public bool IsJumping => !_characterController.isGrounded;

    // 목표: [W],[A],[S],[D] 및 방향키를 누르면 캐릭터를 그 뱡향으로 이동시키고 싶다.
    private CharacterController _characterController;
    private Animator _animator;

    private float _gravity = -9.8f;
    private float _yVelocity = 0f;


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

        // 순서
        // 1. 사용자의 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        _animator.SetFloat("Move", dir.magnitude);

        // 3. 중력 적용하세요.
        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;

        float moveSpeed = Owner.Stat.MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && Owner.Stat.Stamina > 0)
        {
            moveSpeed = Owner.Stat.RunSpeed;
            Owner.Stat.Stamina -= Time.deltaTime * Owner.Stat.RunConsumeStamina;
        }
        else
        {
            Owner.Stat.Stamina += Time.deltaTime * Owner.Stat.RecoveryStamina;
            if (Owner.Stat.Stamina >= Owner.Stat.MaxStamina)
            {
                Owner.Stat.Stamina = Owner.Stat.MaxStamina;
            }
        }

        // 4. 이동속도에 따라 그 방향으로 이동한다.
        _characterController.Move(dir * (moveSpeed * Time.deltaTime));

        // 5. 점프 적용하기
        bool haveJumpStamina = Owner.Stat.Stamina >= Owner.Stat.JumpConsumeStamina;
        if (haveJumpStamina && Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            Owner.Stat.Stamina -= Owner.Stat.JumpConsumeStamina;
            _yVelocity = Owner.Stat.JumpPower;
        }
    }

    public void Teleport(Vector3 position)
    {
        _characterController.enabled = false;

        transform.position = position;

        _characterController.enabled = true;
    }
}
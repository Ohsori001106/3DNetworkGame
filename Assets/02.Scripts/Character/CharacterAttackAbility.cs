using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterAttackAbility : CharacterAbility
{

    // SOLID 법칙: 객체지향 5가지 법칙
    // 1. 단일 책임 원칙 ( 가장 단순하지만 꼭 지켜야 하는 원칙)
    // - 클래스는 단 한개의 책임을 가져야 한다.
    // - 클래스를 변경하는 이유는 단 하나여야 한다.
    // - 이를 지키지 않으면 한 책임 변경에 의해 다른 책임과 관련된 코드도 영향을 미칠 수 있어서
    // -> 유지보수가 매우 어렵다
    // 준수 전략
    // - 기존의 클래스로 해결할 수 없다면 새로운 클래스를 구현
    
    private Animator _animator;
    public Collider WeaponCollider;

    // 때린 애들을 기억해두는 리스트
    private List<IDamaged> _damagedList = new List<IDamaged>();

    private float _attackTimer;

    public GameObject HitEffectPrefab;
    
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Owner.State == State.Death || !Owner.PhotonView.IsMine)
        {
            return;
        }

        _attackTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _attackTimer >=Owner.Stat.AttackCoolTime && Owner.Stat.Stamina >= Owner.Stat.AttackConsumeStamina)
        {
            Owner.Stat.Stamina -= Owner.Stat.AttackConsumeStamina;
            

            //PlayAttackAnumation(Random.Range(1, 4));

            Owner.PhotonView.RPC(nameof(PlayAttackAnumation), RpcTarget.All, Random.Range(1,4));

            _attackTimer = 0;
            // Rpcsrget.All : 모두에게
            // RpcTarget.Others : 나 자신을 제외하고 모두에게
            // RpcTarget.Master : 방장에게만
        }
    }
    
    [PunRPC]
    public void PlayAttackAnumation(int index)
    {
        _animator.SetTrigger($"Attack{index}");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(Owner.PhotonView.IsMine == false|| other.transform == transform )
        {
            return;
        }

        // 0: 개방 폐쇄 원칙
        // 수정에는 닫혀있고, 확장에는 열려있다.
        IDamaged damagedAbleObject = other.GetComponent<IDamaged>();
        if (damagedAbleObject != null) 
        {
            if (_damagedList.Contains(damagedAbleObject))
            {
                return;
            }

            // 안 맞은 애면 때린 리스트에 추가
            _damagedList.Add(damagedAbleObject);


            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null)
            {
                
                photonView.RPC("Damaged", RpcTarget.All, Owner.Stat.Damage);
                
                photonView.RPC("HitEffect", RpcTarget.All, (other.transform.position + transform.position) / 2f);
            }
            // damagedAbleObject.Damaged(Owner.Stat.Damage);
        }
        
    }

    [PunRPC]
    public void HitEffect(Vector3 position)
    {
        Instantiate(HitEffectPrefab, position, Quaternion.identity);
    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
    }
    public void InactiveCollider()
    {
        WeaponCollider.enabled = false;

        // 리스트 초기화
        _damagedList.Clear();
    }
}

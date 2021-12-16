using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Enemy 의 상태를 처리할 구조를 작성
// 대기, 이동, 공격, 피격, 죽음
public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    };

    EnemyState m_state;

    CharacterController cc;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        m_state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        print("현재상태 : " + m_state);
        switch(m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    // 일정시간이 지나면 상태를 Move 로 전환
    // 필요속성 : 대기시간, 경과시간
    public float idleDelayTime = 2;
    float currentTime = 0;
    private void Idle()
    {
        // 일정시간이 지나면 상태를 Move 로 전환
        // 1. 시간이 흘렀으니까
        currentTime += Time.deltaTime;
        // 2. 일정시간이 됐으니까
        if (currentTime > idleDelayTime)
        {
            // 3. 상태를 move 로 전환
            m_state = EnemyState.Move;
            // 애니메이션의 상태도 Move 로 전환
            anim.SetTrigger("Move");
            currentTime = 0;
        }
    }

    // 타겟 방향으로 이동하고 싶다.
    // 필요속성 : 이동속도, 타겟
    public float speed = 5;
    public Transform target;
    // 공격범위안에 타겟이 들어오면 상태를 Attack 으로 전환
    // 필요속성 : 공격범위
    public float attackRange = 2;

    private void Move()
    {
        // 타겟 방향으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // 공격범위안에 타겟이 들어오면 상태를 Attack 으로 전환
        if(distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            return;
        }
        dir.y = 0;
        dir.Normalize();
        // 2. 이동하고 싶다.
        // P = P0 + vt
        cc.SimpleMove(dir * speed);

        // 이동하는 방향으로 회전하고 싶다.
        //transform.LookAt(target);
        //transform.forward = dir;
        // 부드럽게 회전하도록 하자 
        //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // Visual Debugging 을 위한 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // 타겟이 공격범위를 벗어나면 상태를 Move 로 전환
    // 일정시간에 한번씩 공격하고 싶다.
    // 필요속성 : 공격대기시간
    public float attackDelayTime = 2;
    private void Attack()
    {
        // 일정시간에 한번씩 공격하고 싶다.
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
            anim.SetTrigger("Attack");
            print("공격!!!!!");
            //Debug.Log("")
        }


        float distance = Vector3.Distance(transform.position, target.position);
        if(distance > attackRange)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("Move");
        }
    }

    // 일정시간 지나면 상태를 Idle 로 전환
    public float damageDelayTime = 2;
    private void Damage()
    {
        currentTime += Time.deltaTime;
        if(currentTime > damageDelayTime)
        {
            currentTime = 0;
            m_state = EnemyState.Idle;
        }
    }

    // 피격 당했을 때 호출되는 함수
    // hp 갖도록 하고싶다.
    int hp = 3;
    // 만약 hp 가 0 이하면 죽이고
    // 그렇지 않으면 상태를 Idle 로 전환하기
    public void OnDamageProcess()
    {
        hp--;
        if(hp <= 0)
        {
            m_state = EnemyState.Die;
        }
        else
        {
            m_state = EnemyState.Damage;
            anim.SetTrigger("Damage");
            currentTime = 0;
        }
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}

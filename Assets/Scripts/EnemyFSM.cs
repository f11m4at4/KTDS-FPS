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

    // Start is called before the first frame update
    void Start()
    {
        m_state = EnemyState.Idle;
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
            currentTime = 0;
        }
    }

    // 타겟 방향으로 이동하고 싶다.
    // 필요속성 : 이동속도, 타겟
    public float speed = 5;
    public Transform target;
    private void Move()
    {
        // 타겟 방향으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        // 2. 이동하고 싶다.
        // P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}

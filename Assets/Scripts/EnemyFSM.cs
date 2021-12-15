using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Enemy �� ���¸� ó���� ������ �ۼ�
// ���, �̵�, ����, �ǰ�, ����
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
        print("������� : " + m_state);
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

    // �����ð��� ������ ���¸� Move �� ��ȯ
    // �ʿ�Ӽ� : ���ð�, ����ð�
    public float idleDelayTime = 2;
    float currentTime = 0;
    private void Idle()
    {
        // �����ð��� ������ ���¸� Move �� ��ȯ
        // 1. �ð��� �귶���ϱ�
        currentTime += Time.deltaTime;
        // 2. �����ð��� �����ϱ�
        if (currentTime > idleDelayTime)
        {
            // 3. ���¸� move �� ��ȯ
            m_state = EnemyState.Move;
            currentTime = 0;
        }
    }

    // Ÿ�� �������� �̵��ϰ� �ʹ�.
    // �ʿ�Ӽ� : �̵��ӵ�, Ÿ��
    public float speed = 5;
    public Transform target;
    private void Move()
    {
        // Ÿ�� �������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        // 2. �̵��ϰ� �ʹ�.
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

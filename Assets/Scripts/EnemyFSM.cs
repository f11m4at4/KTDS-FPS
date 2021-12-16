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
            // �ִϸ��̼��� ���µ� Move �� ��ȯ
            anim.SetTrigger("Move");
            currentTime = 0;
        }
    }

    // Ÿ�� �������� �̵��ϰ� �ʹ�.
    // �ʿ�Ӽ� : �̵��ӵ�, Ÿ��
    public float speed = 5;
    public Transform target;
    // ���ݹ����ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
    // �ʿ�Ӽ� : ���ݹ���
    public float attackRange = 2;

    private void Move()
    {
        // Ÿ�� �������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // ���ݹ����ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
        if(distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            return;
        }
        dir.y = 0;
        dir.Normalize();
        // 2. �̵��ϰ� �ʹ�.
        // P = P0 + vt
        cc.SimpleMove(dir * speed);

        // �̵��ϴ� �������� ȸ���ϰ� �ʹ�.
        //transform.LookAt(target);
        //transform.forward = dir;
        // �ε巴�� ȸ���ϵ��� ���� 
        //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // Visual Debugging �� ���� �Լ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Ÿ���� ���ݹ����� ����� ���¸� Move �� ��ȯ
    // �����ð��� �ѹ��� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : ���ݴ��ð�
    public float attackDelayTime = 2;
    private void Attack()
    {
        // �����ð��� �ѹ��� �����ϰ� �ʹ�.
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            currentTime = 0;
            anim.SetTrigger("Attack");
            print("����!!!!!");
            //Debug.Log("")
        }


        float distance = Vector3.Distance(transform.position, target.position);
        if(distance > attackRange)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("Move");
        }
    }

    // �����ð� ������ ���¸� Idle �� ��ȯ
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

    // �ǰ� ������ �� ȣ��Ǵ� �Լ�
    // hp ������ �ϰ�ʹ�.
    int hp = 3;
    // ���� hp �� 0 ���ϸ� ���̰�
    // �׷��� ������ ���¸� Idle �� ��ȯ�ϱ�
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        m_state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //print("������� : " + m_state);
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
                //Damage();
                break;
            case EnemyState.Die:
                //Die();
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

    float runSpeed = 0;
    private void Move()
    {
        if(agent.enabled == false)
        {
            agent.enabled = true;
        }
        // 1. ������ �ʿ�
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // ���ݹ����ȿ� Ÿ���� ������ ���¸� Attack ���� ��ȯ
        if(distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            // ��ã�� ����
            agent.enabled = false;
            return;
        }

        // Agent �� �̿��� ��ã��
        agent.destination = target.position;
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
    // Damage �Լ��� �ڷ�ƾ�� ����� �ʹ�.
    private IEnumerator Damage()
    {
        // 1. ���¸� Damage �� ��ȯ
        m_state = EnemyState.Damage;
        // 2. �ִϸ��̼� Damage ���·� ��ȯ
        anim.SetTrigger("Damage");
        // 3. ��� ��ٸ���
        yield return new WaitForSeconds(damageDelayTime);
        // 4. ��ٸ����� ���¸� Idle �� ��ȯ
        m_state = EnemyState.Idle;

    }

    // �ǰ� ������ �� ȣ��Ǵ� �Լ�
    // hp ������ �ϰ�ʹ�.
    int hp = 3;
    // ���� hp �� 0 ���ϸ� ���̰�
    // �׷��� ������ ���¸� Idle �� ��ȯ�ϱ�
    public void OnDamageProcess()
    {
        // �̹� ���°� Die �̸� ȣ����� �ʵ�������
        if(m_state == EnemyState.Die)
        {
            return;
        }

        agent.enabled = false;

        hp--;
        currentTime = 0;

        StopAllCoroutines();
       // StopCoroutine("Damage");

        if(hp <= 0)
        {
            StartCoroutine(Die());
            print("205");
        }
        else
        {
            // �ڷ�ƾ ����(���)
            //StartCoroutine(Damage());
            StartCoroutine("Damage");
        }
    }

    // �Ʒ��� ��� �������ٰ� �Ⱥ��̸� ���Ž�������
    // �ʿ�Ӽ� : �������ӵ�, ����� ��ġ
    public float dieSpeed = 0.5f;
    public float dieYPosition = -2;
    private IEnumerator Die()
    {
        m_state = EnemyState.Die;
        anim.SetTrigger("Die");
        // �浹ü ������Ű��
        cc.enabled = false;

        yield return new WaitForSeconds(2);

        // �Ʒ��� ����ɵ��� ����
        // P = P0 + vt
        while (transform.position.y > dieYPosition)
        {
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);

    }
}

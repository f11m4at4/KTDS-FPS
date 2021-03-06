using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        //print("현재상태 : " + m_state);
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

    float runSpeed = 0;
    private void Move()
    {
        if(agent.enabled == false)
        {
            agent.enabled = true;
        }
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // 공격범위안에 타겟이 들어오면 상태를 Attack 으로 전환
        if(distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            // 길찾기 종료
            agent.enabled = false;
            return;
        }

        // Agent 를 이용한 길찾기
        agent.destination = target.position;
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
    // Damage 함수를 코루틴을 만들고 싶다.
    private IEnumerator Damage()
    {
        // 1. 상태를 Damage 로 전환
        m_state = EnemyState.Damage;
        // 2. 애니메이션 Damage 상태로 전환
        anim.SetTrigger("Damage");
        // 3. 잠시 기다리기
        yield return new WaitForSeconds(damageDelayTime);
        // 4. 기다린다음 상태를 Idle 로 전환
        m_state = EnemyState.Idle;

    }

    // 피격 당했을 때 호출되는 함수
    // hp 갖도록 하고싶다.
    int hp = 3;
    // 만약 hp 가 0 이하면 죽이고
    // 그렇지 않으면 상태를 Idle 로 전환하기
    public void OnDamageProcess()
    {
        // 이미 상태가 Die 이면 호출되지 않도록하자
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
            // 코루틴 시작(등록)
            //StartCoroutine(Damage());
            StartCoroutine("Damage");
        }
    }

    // 아래로 계속 내려가다가 안보이면 제거시켜주자
    // 필요속성 : 죽을때속도, 사라질 위치
    public float dieSpeed = 0.5f;
    public float dieYPosition = -2;
    private IEnumerator Die()
    {
        m_state = EnemyState.Die;
        anim.SetTrigger("Die");
        // 충돌체 정지시키자
        cc.enabled = false;

        yield return new WaitForSeconds(2);

        // 아래로 가라앉도록 하자
        // P = P0 + vt
        while (transform.position.y > dieYPosition)
        {
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);

    }
}

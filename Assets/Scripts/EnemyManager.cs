using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 랜덤시간에 한번씩 랜덤한 위치에서 적이 생성되도록 하고싶다.
// 필요속성 : 랜덤시간 간격, Spawn 될 위치들
public class EnemyManager : MonoBehaviour
{
    // 필요속성 : 랜덤시간 간격, Spawn 될 위치들
    public float minTime = 0.5f;
    public float maxTime = 3.5f;
    float createTime = 0;

    public Transform[] spawnPoints;
    public GameObject enemyFactory;
    // Start is called before the first frame update
    void Start()
    {
        // 랜덤시간에 한번씩 랜덤한 위치에서 적이 생성되도록 하고싶다.
        // 1. 랜덤한 시간을 구해야 한다.
        createTime = Random.Range(minTime, maxTime);

        Invoke("CreateEnemy", createTime);
    }

    void CreateEnemy()
    {
        // 3. 랜덤한 위치
        int index = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[index].position;
        // 4. 적이 생성되도록 하고싶다.
        GameObject enemy = Instantiate(enemyFactory);
        enemy.transform.position = pos;

        createTime = Random.Range(minTime, maxTime);

        Invoke("CreateEnemy", createTime);
    }
}

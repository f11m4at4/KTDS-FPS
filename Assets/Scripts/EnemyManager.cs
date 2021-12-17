using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ð��� �ѹ��� ������ ��ġ���� ���� �����ǵ��� �ϰ�ʹ�.
// �ʿ�Ӽ� : �����ð� ����, Spawn �� ��ġ��
public class EnemyManager : MonoBehaviour
{
    // �ʿ�Ӽ� : �����ð� ����, Spawn �� ��ġ��
    public float minTime = 0.5f;
    public float maxTime = 3.5f;
    float createTime = 0;

    public Transform[] spawnPoints;
    public GameObject enemyFactory;
    // Start is called before the first frame update
    void Start()
    {
        // �����ð��� �ѹ��� ������ ��ġ���� ���� �����ǵ��� �ϰ�ʹ�.
        // 1. ������ �ð��� ���ؾ� �Ѵ�.
        createTime = Random.Range(minTime, maxTime);

        Invoke("CreateEnemy", createTime);
    }

    void CreateEnemy()
    {
        // 3. ������ ��ġ
        int index = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[index].position;
        // 4. ���� �����ǵ��� �ϰ�ʹ�.
        GameObject enemy = Instantiate(enemyFactory);
        enemy.transform.position = pos;

        createTime = Random.Range(minTime, maxTime);

        Invoke("CreateEnemy", createTime);
    }
}

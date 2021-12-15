using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. ������� �Է¿����� �յ��¿�� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�, CharacterController
// 2. �߷��� ����ǵ��� �ϰ� �ʹ�.
// �ʿ�Ӽ� : �߷�, �����ӵ� ����
// 3. ����ڰ� ������ư�� ������ ������ �ϰ� �ʹ�.
// �ʿ�Ӽ� : �����Ŀ�
// - Mission : 2�� ���������� �ϰ�ʹ�.
// �ʿ�Ӽ� : �������� ����, �ִ� ���� ����
public class PlayerMove : MonoBehaviour
{
    // �ʿ�Ӽ� : �̵��ӵ�
    public float speed = 5;
    CharacterController cc;

    // �ʿ�Ӽ� : �߷�, �����ӵ� ����
    public float gravity = 20;
    float yVelocity = 0;
    // �ʿ�Ӽ� : �����Ŀ�
    public float jumpPower = 10;
    // �ʿ�Ӽ� : �������� ����, �ִ� ���� ����
    int jumpCount = 0;
    public int maxJumpCount = 2;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // ������� �Է¿����� �յ��¿�� �̵��ϰ� �ʹ�.
        // 1. ������� �Է¿� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. �������ʿ�
        Vector3 dir = new Vector3(h, 0, v);
        // ������ ī�޶� ���ϴ� �������� ��ȯ
        dir = Camera.main.transform.TransformDirection(dir);
        // v = v0 + at

        // �ٴڿ� ��� ���� �� �����ӵ��� 0���� ������Ѵ�.
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // 3. ����ڰ� ������ư�� ������ ������ �ϰ� �ʹ�.
        if(jumpCount < maxJumpCount && Input.GetButtonDown("Jump"))
        {
            // - Mission : 2�� ���������� �ϰ�ʹ�.
            jumpCount++;
            yVelocity = jumpPower;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        // 3. �̵��ϰ�ʹ�.
        // P = P0 + vt
        cc.Move(dir * speed * Time.deltaTime);
    }
}

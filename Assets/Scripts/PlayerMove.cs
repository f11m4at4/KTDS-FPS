using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 사용자의 입력에따라 앞뒤좌우로 이동하고 싶다.
// 필요속성 : 이동속도, CharacterController
// 2. 중력이 적용되도록 하고 싶다.
// 필요속성 : 중력, 수직속도 성분
// 3. 사용자가 점프버튼을 누르면 점프를 하고 싶다.
// 필요속성 : 점프파워
// - Mission : 2단 점프까지만 하고싶다.
// 필요속성 : 현재점프 갯수, 최대 점프 갯수
public class PlayerMove : MonoBehaviour
{
    // 필요속성 : 이동속도
    public float speed = 5;
    CharacterController cc;

    // 필요속성 : 중력, 수직속도 성분
    public float gravity = 20;
    float yVelocity = 0;
    // 필요속성 : 점프파워
    public float jumpPower = 10;
    // 필요속성 : 현재점프 갯수, 최대 점프 갯수
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
        // 사용자의 입력에따라 앞뒤좌우로 이동하고 싶다.
        // 1. 사용자의 입력에 따라
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 방향이필요
        Vector3 dir = new Vector3(h, 0, v);
        // 방향을 카메라가 향하는 방향으로 변환
        dir = Camera.main.transform.TransformDirection(dir);
        // v = v0 + at

        // 바닥에 닿아 있을 때 수직속도를 0으로 해줘야한다.
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        // 3. 사용자가 점프버튼을 누르면 점프를 하고 싶다.
        if(jumpCount < maxJumpCount && Input.GetButtonDown("Jump"))
        {
            // - Mission : 2단 점프까지만 하고싶다.
            jumpCount++;
            yVelocity = jumpPower;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        // 3. 이동하고싶다.
        // P = P0 + vt
        cc.Move(dir * speed * Time.deltaTime);
    }
}

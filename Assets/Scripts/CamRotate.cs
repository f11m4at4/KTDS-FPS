using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력에따라 물체를 회전하고 싶다. 
// 필요속성 : 회전속도
public class CamRotate : MonoBehaviour
{
    // 필요속성 : 회전속도
    [SerializeField]
    private float rotSpeed = 200;

    float mx;
    float my;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 사용자의 입력에따라 물체를 회전하고 싶다. 
        // 1. 사용자의 입력에따라
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // P = P0 + vt
        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -60, 60);
        // 2. 방향이 필요
        Vector3 dir = new Vector3(-my, mx, 0);
        // 3. 물체를 회전하고 싶다.
        transform.eulerAngles = dir;

    }
}

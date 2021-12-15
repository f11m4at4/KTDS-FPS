using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է¿����� ��ü�� ȸ���ϰ� �ʹ�. 
// �ʿ�Ӽ� : ȸ���ӵ�
public class CamRotate : MonoBehaviour
{
    // �ʿ�Ӽ� : ȸ���ӵ�
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
        // ������� �Է¿����� ��ü�� ȸ���ϰ� �ʹ�. 
        // 1. ������� �Է¿�����
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // P = P0 + vt
        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -60, 60);
        // 2. ������ �ʿ�
        Vector3 dir = new Vector3(-my, mx, 0);
        // 3. ��ü�� ȸ���ϰ� �ʹ�.
        transform.eulerAngles = dir;

    }
}

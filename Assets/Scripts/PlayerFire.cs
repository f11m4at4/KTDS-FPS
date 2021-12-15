using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ڰ� �߻��ư�� ������ ���� �߻��ϰ� �ʹ�.
// �ʿ�Ӽ� : �ѱ�(ī�޶�)
public class PlayerFire : MonoBehaviour
{
    // �ʿ�Ӽ� : ��������Ʈ
    public GameObject bulletEffectFactory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ����ڰ� �߻��ư�� ������ ���� �߻��ϰ� �ʹ�.
        // 1. ����ڰ� �߻��ư�� �������ϱ�
        if (Input.GetButtonDown("Fire1"))
        {
            // 2. ���� �߻��ϰ� �ʹ�.
            // 1) Ray �� �ʿ��ϴ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            // 2) Ray�� ������ �Ѵ�.
            bool result = Physics.Raycast(ray, out hitInfo);
            // 3) Ray �� ���� ������ ������ Ƣ�� �ϰ� �ʹ�.
            if(result)
            {
                // ������ ���忡�� �����.
                GameObject bulletEffect = Instantiate(bulletEffectFactory);
                // ������ ��ġ��Ų��.
                bulletEffect.transform.position = hitInfo.point;

            }
        }
    }
}

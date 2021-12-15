using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자가 발사버튼을 누르면 총을 발사하고 싶다.
// 필요속성 : 총구(카메라)
public class PlayerFire : MonoBehaviour
{
    // 필요속성 : 파편이펙트
    public GameObject bulletEffectFactory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자가 발사버튼을 누르면 총을 발사하고 싶다.
        // 1. 사용자가 발사버튼을 눌렀으니까
        if (Input.GetButtonDown("Fire1"))
        {
            // 2. 총을 발사하고 싶다.
            // 1) Ray 가 필요하다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;

            // 내가 맞추고 싶은 녀석만 맞추도록 하고 싶다.
            int layer = gameObject.layer;
            layer = 1 << layer;

            // 2) Ray를 던져야 한다.
            bool result = Physics.Raycast(ray, out hitInfo, 100, ~layer);
            // 3) Ray 가 닿은 지점에 파편을 튀게 하고 싶다.
            if(result)
            {
                // 파편을 공장에서 만든다.
                GameObject bulletEffect = Instantiate(bulletEffectFactory);
                // 파편을 위치시킨다.
                bulletEffect.transform.position = hitInfo.point;
                // 부딪힌 지점이 향하는 방향으로 파편을 튀게하고 싶다.
                bulletEffect.transform.forward = hitInfo.normal;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Transform needles;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            // 플레이어 충돌감지 시 위로 올린다.
            Vector3 pos = needles.transform.position;
            pos.y = 18;
            needles.transform.position = pos;
        }
    }
}

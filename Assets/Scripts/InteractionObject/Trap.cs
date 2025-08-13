using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Transform needles;
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private LayerMask playerLayer;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, playerLayer))
        {
            Vector3 pos = needles.transform.position;
            pos.y = 18;
            needles.transform.position = pos;
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 9)
    //    {
    //        // 플레이어 충돌감지 시 위로 올린다.
    //        Vector3 pos = needles.transform.position;
    //        pos.y = 18;
    //        needles.transform.position = pos;
    //    }
    //}
}

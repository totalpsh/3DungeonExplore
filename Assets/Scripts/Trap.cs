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
            // �÷��̾� �浹���� �� ���� �ø���.
            Vector3 pos = needles.transform.position;
            pos.y = 18;
            needles.transform.position = pos;
        }
    }
}

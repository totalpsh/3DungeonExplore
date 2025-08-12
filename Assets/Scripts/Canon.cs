using System.Collections;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] Rigidbody targetRigid;
    [SerializeField] Transform firepoint;

    private float power = 300f;
    private float time = 2f;

    Coroutine coroutine;

    private void OnCollisionEnter(Collision collision)
    {
        targetRigid = collision.gameObject.GetComponent<Rigidbody>();

        coroutine = StartCoroutine(CanonFire());
    }

    private IEnumerator CanonFire()
    {
        float curTime = time;

        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }
        targetRigid.velocity = Vector3.zero;
        targetRigid.AddForce(new Vector3(0, 1, 1) * power, ForceMode.Impulse);
    }

}

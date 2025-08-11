using UnityEngine;

public class JumpPlatform : MonoBehaviour
{

    [SerializeField] private Rigidbody targetRb;
    private float power = 300f;


    private void OnCollisionEnter(Collision collision)
    {
        targetRb = collision.gameObject.GetComponent<Rigidbody>();

        targetRb.AddForce(Vector3.up * power, ForceMode.Impulse);
    }
}

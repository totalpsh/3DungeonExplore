using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    private Vector3 startPos;
    private Transform curPos;
    private Vector3 lastPos;

    private bool isRight;

    [SerializeField] Rigidbody targetRigid;
    [SerializeField] float maxMovingDistance;
    [SerializeField] float movingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        curPos = transform;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();

        Vector3 delta = transform.position - lastPos;

        if (targetRigid != null)
        {
            targetRigid.MovePosition(targetRigid.position + delta);
        }

        lastPos = transform.position;
    }

    void MovePlatform()
    {
        if(isRight)
        {
            curPos.position = new Vector3(curPos.position.x - movingSpeed, transform.position.y, transform.position.z);

            if (startPos.x - curPos.position.x >= maxMovingDistance) isRight = false;
        }
        else
        {
            curPos.position = new Vector3(curPos.position.x + movingSpeed, transform.position.y, transform.position.z);

            if (startPos.x <= curPos.position.x) isRight = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            targetRigid = collision.gameObject.GetComponent<Rigidbody>();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            targetRigid = null;
        }
    }
}

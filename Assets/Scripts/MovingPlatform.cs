using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
    private Transform curPos;

    private bool isRight;

    [SerializeField] float maxMovingDistance;
    [SerializeField] float movingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        curPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
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


        //float step = movingSpeed * Time.fixedDeltaTime;

        //Vector3 pos = _rigidbody.position;

        //if (isRight)
        //{
        //    pos.x -= step;
        //    if (startPos.x - pos.x >= maxMovingDistance) isRight = false;
        //}
        //else
        //{
        //    pos.x += step;
        //    if (startPos.x <= pos.x) isRight = true;
        //}

        //_rigidbody.MovePosition(pos);
    }


}

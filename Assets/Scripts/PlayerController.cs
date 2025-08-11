using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask grounLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private float rotationSpeedDeg = 720f;
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    private Camera _camera;

    private bool _isGrounded;

    private float useStamina = 10;

    private void Awake()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, grounLayerMask);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        //CameraLook();
    }

    void Move()
    {
        Vector3 camForward = _camera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = _camera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = (camForward * curMovementInput.y + camRight * curMovementInput.x);
        bool hasInput = moveDir.sqrMagnitude > 0.0001f;
        if (hasInput) { moveDir.Normalize(); }

        if (hasInput)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion newRot = Quaternion.RotateTowards(_rigidbody.rotation, targetRot, rotationSpeedDeg * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(newRot);
        }

        Vector3 dir = _rigidbody.velocity;
        Vector3 horizontal = hasInput ? moveDir * moveSpeed : Vector3.zero;
        dir.x = horizontal.x;
        dir.z = horizontal.z;

        _rigidbody.velocity = dir;

        //Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        //dir *= moveSpeed;
        //dir.y = _rigidbody.velocity.y;

        //_rigidbody.velocity = dir;
    }

    //void CameraLook()
    //{
    //    camCurXRot += mouseDelta.y * lookSensitivity;
    //    camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
    //    cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

    //    transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    //}

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }    

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && _isGrounded)
        {
            if(CharacterManager.Instance.Player.condition.UseStamina(useStamina))
                _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
}

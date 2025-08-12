using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask grounLayerMask;
    public bool isClimbing;
    public bool canClimb;
    public Vector3 wallNormal;
    public bool canDoubleJump;
    public int jumpCount;

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

    private float useJumpStamina = 10;
    private float useClimbStamina = 0.5f;

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
        if(_isGrounded && canDoubleJump && jumpCount <= 0)
        {
            jumpCount = 1;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isClimbing) Climbing();
        else Move();
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
        if (hasInput) moveDir.Normalize(); 

        if (hasInput)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion newRot = Quaternion.RotateTowards(_rigidbody.rotation, targetRot, rotationSpeedDeg * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(newRot);
        }

        Vector3 dir = _rigidbody.velocity;

        if (hasInput)
        {
            Vector3 horizontal = hasInput ? moveDir * moveSpeed : Vector3.zero;
            dir.x = horizontal.x;
            dir.z = horizontal.z;
        }

        _rigidbody.velocity = dir;

        //Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        //dir *= moveSpeed;
        //dir.y = _rigidbody.velocity.y;

        //_rigidbody.velocity = dir;

    }

    void Climbing()
    {
        if (CharacterManager.Instance.Player.condition.UseStamina(useClimbStamina))
        {

            Vector3 upMove = transform.up * curMovementInput.y * moveSpeed;
            Vector3 sideMove = Vector3.Cross(wallNormal, Vector3.up) * curMovementInput.x * moveSpeed;
            Vector3 moveDir = upMove + sideMove;

            _rigidbody.MovePosition(_rigidbody.position + moveDir * Time.fixedDeltaTime);
        }
        else
        {
            StopClimbing();
        }
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
        if(context.phase == InputActionPhase.Started  && !isClimbing)
        {
            if (_isGrounded)
            {
                if (CharacterManager.Instance.Player.condition.UseStamina(useJumpStamina))
                {
                    _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
                }
            }
            if (!_isGrounded  && canDoubleJump && jumpCount > 0)
            {
                if (CharacterManager.Instance.Player.condition.UseStamina(useJumpStamina))
                {
                    _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
                    jumpCount--;
                }
            }
        }
        else if(isClimbing)
        {
            StopClimbing();
        }
    }

    public void OnClimbing(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && canClimb)
        {
            _rigidbody.useGravity = false;
            isClimbing = true;
        }
    }

    public void StopClimbing()
    {
        _rigidbody.useGravity = true;
        isClimbing = false;
    }

    public void DoubleJumpStart()
    {
        canDoubleJump = true;
        jumpCount = 1;
    }

    public void DoubleJumpStop()
    {
        canDoubleJump = false;
    }
}

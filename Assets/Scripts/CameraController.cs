using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float followLerp = 15f;
    [SerializeField] private Vector3 followOffset = Vector3.zero;

    [Header("MouseLook")]
    [SerializeField] private Transform cam;
    [SerializeField] private float distance = 10f;
    [SerializeField] float yawSpeed = 100f;
    [SerializeField] float pitchSpeed = 60f;
    [SerializeField] float minPitch = -30f, maxPitch = 70f;
    private Vector2 mouseDelta;

    private float currentDistance;
    private float yaw, pitch;

    // Start is called before the first frame update
    void Start()
    {
        if (!cam) cam = Camera.main.transform;

        // 원하면 커서 락
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 초기 시선 세팅 (원하면 생략)
        Vector3 look = (target ? target.forward : Vector3.forward);
        yaw = Mathf.Atan2(look.x, look.z) * Mathf.Rad2Deg;
        pitch = 10f;
    }

    private void LateUpdate()
    {
        if (!target || !cam) return;

        // 1) 타겟 위치만 따라감
        Vector3 targetPos = target.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followLerp * Time.deltaTime);

        // 2) 마우스 입력으로 yaw/pitch 갱신 (플레이어 회전과 무관)
        yaw += mouseDelta.x * yawSpeed * Time.deltaTime;
        pitch -= mouseDelta.y * pitchSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 4) 카메라 충돌 처리 (스프링암 느낌)
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredCamPos = transform.position - rot * Vector3.forward * distance;

        cam.position = transform.position - rot * Vector3.forward * distance;
        cam.rotation = rot;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}

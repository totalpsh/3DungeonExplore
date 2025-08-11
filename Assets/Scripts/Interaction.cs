#define ThirdPersonRayCast

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;


public class Interaction : MonoBehaviour
{
    // #define Test
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    // 3인칭 시점에서 사용할 Rigidbody
    public Rigidbody _rigidbody;

    // 3인칭 시점에서 사용할 콜라이더
    public BoxCollider boxCollider;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;

        boxCollider = GetComponentInChildren<BoxCollider>();

        _rigidbody = GetComponent<Rigidbody>();
    }

#if(FirstPersonRayCast)
    private void Update()
    {
        //1인칭 시점 카메라의 중앙에서 Ray를 쏘는 방식
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPrompt();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }

        }


    }
#elif (ThirdPersonRayCast)
    private void Update()
    {
        // 3인칭 시점 몸에서 Ray를 쏘도록 한다.
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {

                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPrompt();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

#elif (ThirdPersonCollider)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (other.gameObject != curInteractGameObject)
            {
                curInteractGameObject = other.gameObject;
                curInteractable = other.GetComponent<IInteractable>();
                SetPrompt();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {

        curInteractGameObject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);

    }
#endif

    private void SetPrompt()  
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null) // 해당 키가 다운 상태이면서 현재 상호작용할 아이템이 null이 아니라면
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);

        }
    }
}

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

    // 3��Ī �������� ����� Rigidbody
    public Rigidbody _rigidbody;

    // 3��Ī �������� ����� �ݶ��̴�
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
        //1��Ī ���� ī�޶��� �߾ӿ��� Ray�� ��� ���
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
        // 3��Ī ���� ������ Ray�� ��� �Ѵ�.
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
        if(context.phase == InputActionPhase.Started && curInteractable != null) // �ش� Ű�� �ٿ� �����̸鼭 ���� ��ȣ�ۿ��� �������� null�� �ƴ϶��
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);

        }
    }
}

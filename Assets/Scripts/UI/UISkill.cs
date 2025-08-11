using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UISkill : MonoBehaviour
{
    public UISkillSlot[] slots;

    public GameObject skillWindow;
    public Transform skillPanel;

    private PlayerController controller;
    private PlayerCondition condition;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.GetComponent<PlayerController>();
        condition = CharacterManager.Instance.Player.GetComponent<PlayerCondition>();
        CharacterManager.Instance.Player.addItem += AddSkill;

        slots = new UISkillSlot[skillPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = skillPanel.GetChild(i).GetComponent<UISkillSlot>();
            slots[i].index = i;
            slots[i].skills = this;
            slots[i].Clear();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill()
    {
        ItemData data = CharacterManager.Instance.Player.itemdata;

        if (CharacterManager.Instance.Player.itemdata.type == ItemType.Consumable)
        {
            UISkillSlot emptySlot = GetEmptySlot();

            if (emptySlot != null)
            {
                emptySlot.item = data;
                UpdateUI();
                CharacterManager.Instance.Player.itemdata = null;
                return;
            }

            CharacterManager.Instance.Player.itemdata = null;
        }
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null) slots[i].Set();
            else slots[i].Clear();
        }
    }

    UISkillSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) return slots[i];
        }

        return null;
    }

    public void OnUseKey(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string path = context.control.path;

            if (path == "/Keyboard/z")
            {
                if (slots[0].item == null) return;

                if (slots[0].item.type == ItemType.Consumable)
                {
                    for(int i = 0; i < slots[0].item.consumables.Length; i++)
                    {
                        switch (slots[0].item.consumables[i].type)
                        {
                            case ConsumableType.Speed:
                                Debug.Log("속도증가");
                                coroutine = StartCoroutine(SpeedUp(slots[0].item.consumables[i].time));
                                break;
                            case ConsumableType.Jump:
                                coroutine = StartCoroutine(JumpPowerUp(slots[0].item.consumables[i].time));
                                break;
                        }
                    }
                }

                slots[0].Clear();
            }
            else if (path == "/Keyboard/x")
            {
                if (slots[1].item == null) return;

                if (slots[1].item.type == ItemType.Consumable)
                {
                    for (int i = 0; i < slots[1].item.consumables.Length; i++)
                    {
                        switch (slots[1].item.consumables[i].type)
                        {
                            case ConsumableType.Speed:
                                coroutine = StartCoroutine(SpeedUp(slots[1].item.consumables[i].time));
                                break;
                            case ConsumableType.Jump:
                                coroutine = StartCoroutine(JumpPowerUp(slots[1].item.consumables[i].time));
                                break;
                        }
                    }
                }

                slots[1].Clear();
            }
            UpdateUI();
        }
    }

    private IEnumerator SpeedUp(float time)
    {
        float originalSpeed = controller.moveSpeed;
        controller.moveSpeed = originalSpeed * 2f;

        float curTime = time;

        while(curTime > 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }

        controller.moveSpeed = originalSpeed;
        coroutine = null;
    }

    private IEnumerator JumpPowerUp(float time)
    {
        float originalJump = controller.jumpPower;
        controller.jumpPower = originalJump * 2f;

        float curTime = time;

        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }

        controller.jumpPower = originalJump;
        coroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    public ItemData item;

    public UISkill skills;
    public TextMeshProUGUI text;

    public int index;

    private void Start()
    {
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void Set()
    {
        text.text = (item != null ) ? item.displayName : string.Empty;
    }

    public void Clear()
    {
        item = null;
        if(text != null) text.text = string.Empty;
    }

    public void OnUseButton()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float currentValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetPercent();
    }

    private float GetPercent()
    {
        return currentValue / maxValue;
    }

    public void Add(float value)
    {
        currentValue = Mathf.Min(currentValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        currentValue = Mathf.Max(currentValue - value, 0);
    }
}

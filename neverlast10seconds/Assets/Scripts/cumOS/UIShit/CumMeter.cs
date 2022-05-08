using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CumMeter : NonInstantiatingSingleton<CumMeter>
{
    // impl for NonInstantiatingSingleton
    protected override CumMeter GetInstance()
    {
        return this;
    }
    
    public float currentCumValue;
    public float amountToCumAt;

    private Image cumMeterImage;
    
    void Start()
    {
        cumMeterImage = GetComponent<Image>();
    }

    public void AddToCumValue(float amount)
    {
        currentCumValue += amount;

        if (currentCumValue >= amountToCumAt)
        {
            TimeToCum();   
        }
    }

    public void RemoveFromCumValue(float amount)
    {
        currentCumValue -= amount;

        if (currentCumValue < 0)
        {
            currentCumValue = 0;
        }
        
    }

    public void UpdateCumMeterUI()
    {
        cumMeterImage.fillAmount = currentCumValue;
    }

    /// <summary>
    /// Actual Cum Time!!!
    /// </summary>
    public void TimeToCum()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class CumMeter : NonInstantiatingSingleton<CumMeter>
{
    public AudioSource cumNoise;
    public float scoreTimer; 

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
        scoreTimer = 0; 
    }

    public void Update()
    {
        scoreTimer += Time.deltaTime; 
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
        StartCoroutine("CumSequence");
    }

    IEnumerator CumSequence()
    {
        //freeze timer
        scoreTimer = scoreTimer;
        //set score for highscore screen
        RetryButton.scoreThisTime = scoreTimer;

        //close all popups and browser tabs?

        cumNoise.Play();

        //activate deflated face
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("Scores"); 

        yield return null; 
    }
}

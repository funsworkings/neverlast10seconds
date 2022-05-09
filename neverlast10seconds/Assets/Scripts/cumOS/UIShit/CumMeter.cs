using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class CumMeter : NonInstantiatingSingleton<CumMeter>
{
    public AudioSource cumNoise;
    public float scoreTimer;
    public Text timerText;

    public float movementMultiplier;
    public float timeMultiplier;

    public Text cumValueText; 

    // impl for NonInstantiatingSingleton
    protected override CumMeter GetInstance()
    {
        return this;
    }
    
    public float currentCumValue;
    public float amountToCumAt = 1;

    public Slider cumMeterUI; 
    
    void Start()
    {
        currentCumValue = 0f; 
        scoreTimer = 0; 
    }

    public void Update()
    {
        scoreTimer += Time.deltaTime;
        currentCumValue -= (ControlHandPosition.amountmousemoved * movementMultiplier);
        currentCumValue += Time.deltaTime * timeMultiplier; 

       // Debug.Log("current cum value: " + currentCumValue.ToString());

        if (currentCumValue >= amountToCumAt)
        {
            StartCoroutine("CumSequence");
        }
        cumMeterUI.value = currentCumValue;

        if (currentCumValue < 0)
        {
            currentCumValue = 0; 
        }
        if (currentCumValue >= 1)
        {
            currentCumValue = 1; 
        }

        timerText.text = "time: " + Mathf.Round(scoreTimer).ToString();

        cumValueText.text = "cum value: " + currentCumValue.ToString();

        Shader.SetGlobalFloat("GlobalTime", Time.time);
    }
    /*
    public void AddToCumValue(float amount)
    {
        currentCumValue += amount;

       
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
    */

    /// <summary>
    /// Actual Cum Time!!!
    /// </summary>
   

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [Header("Cum Ending")]
    public bool playerCame;
    public UnityEvent onPlayerCumEvent;
    public GameObject computerInterface;
    public GameObject scoreInterface;
    
    void Start()
    {
        currentCumValue = 0f; 
        scoreTimer = 0; 
    }

    public void Update()
    {
        if (currentCumValue >= amountToCumAt)
        {
            TimeToCum();
        }

        if (!playerCame)
        {
            scoreTimer += Time.deltaTime;
            currentCumValue -= (ControlHandPosition.amountmousemoved * movementMultiplier);
            currentCumValue += Time.deltaTime * timeMultiplier; 
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
        }
        Shader.SetGlobalFloat("GlobalTime", Time.time);
        // Debug.Log("current cum value: " + currentCumValue.ToString());
    }

    /// <summary>
    /// Actual Cum Time!!!
    /// </summary>
    public void TimeToCum()
    {
        playerCame = true;
        
        onPlayerCumEvent?.Invoke();
        
        StartCoroutine("CumSequence");
    }

    IEnumerator CumSequence()
    {
        //close all popups and browser tabs?

        cumNoise.Play();
        
        computerInterface.SetActive(false);

        //activate deflated face
        yield return new WaitForSecondsRealtime(3);
        
        scoreInterface.SetActive(true);
    }
}

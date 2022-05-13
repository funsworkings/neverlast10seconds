using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CumMeter : NonInstantiatingSingleton<CumMeter>
{
    public AudioSource cumNoise;
    public float scoreTimer;
    public TMP_Text timerText;

    public float movementMultiplier;
    public float timeMultiplier;

    public TMP_Text cumValueText; 

    // impl for NonInstantiatingSingleton
    protected override CumMeter GetInstance()
    {
        return this;
    }
    
    public float currentCumValue;
    public float amountToCumAt = 1;

    public Slider cumMeterUI;
    [Header("Cum Ending")] 
    public bool hasStarted;
    public bool playerCame;
    public UnityEvent onBeginMasturbationEvent;
    public UnityEvent onPlayerCumEvent;
    public GameObject titleInterface;
    public GameObject cumInterface;
    public GameObject computerInterface;
    public GameObject scoreInterface;

    public AnimationCurve cumCurve;

    private float cumRate;
    
    void Start()
    {
        currentCumValue = 0f; 
        scoreTimer = 0; 
        //enable only title interface
        titleInterface.SetActive(true);
        scoreInterface.SetActive(false);
        cumInterface.SetActive(false);
        computerInterface.SetActive(false);
    }

    public void SetStarted()
    {
        computerInterface.SetActive(true);
        cumInterface.SetActive(true);
        titleInterface.SetActive(false);
        hasStarted = true;
        onBeginMasturbationEvent?.Invoke();
    }

    public void Update()
    {
        if (currentCumValue >= amountToCumAt)
        {
            TimeToCum();
        }

        //to start from title 
        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetStarted();
            }
        }

        if (!playerCame && hasStarted)
        {
            scoreTimer += Time.deltaTime;
            currentCumValue -= (ControlHandPosition.amountmousemoved * movementMultiplier);
            currentCumValue += Time.deltaTime * timeMultiplier * (1 + (currentCumValue * 3)); 
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

        cumRate += Time.deltaTime * cumCurve.Evaluate(currentCumValue);
        Shader.SetGlobalFloat("GlobalTime", cumRate);
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

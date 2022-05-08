using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DonkeyGameManager : NonInstantiatingSingleton<DonkeyGameManager>
{
    
    // impl for NonInstantiatingSingleton
    protected override DonkeyGameManager GetInstance()
    {
        return this;
    }

    private DonkeySoundFX donkeySoundFx;
    private Vector3 origScale;

    [Tooltip("Shows us how far along donkey is to cumming")]
    public float cumMultiplier;
    [Tooltip("Cum multi must reach this amount to cum")]
    public float levelToCumAt;

    public ParticleSystem cumParticles;
    
    public float cumResetTime;
    public Vector2 cumResetRange = new Vector2(5f, 10f);

    [Tooltip("Possible increase in cum cock length")]
    public Vector2 cumIncreaseRange = new Vector2(1f, 2f);
    [Tooltip("The donkey cock model")]
    public Transform donkeyCock;

    [Tooltip("Tells the player what to press to cum")]
    public Image controlsUIimage;
    public Sprite[] arrowSprites;
    [Tooltip("Tells the player what to press to cum")]
    public TMP_Text youCameText;

    public KeyCode currentArrowExpectation;
    public KeyCode[] possibleInputs;
    void Start()
    {
        donkeySoundFx = GetComponent<DonkeySoundFX>();
        origScale = donkeyCock.localScale;
        controlsUIimage.enabled = true;
        youCameText.enabled = false;
        ResetCumMeter();
    }

    void Update()
    {
        if (Input.GetKeyDown(currentArrowExpectation))
        {
            IncreaseCumLevel();
           
            if(donkeySoundFx)
                donkeySoundFx.PlayRandomSoundRandomPitch(donkeySoundFx.donkeyInputSounds, 1f);
        }
    }

    void ResetCumMeter()
    {
        cumMultiplier = 1f;
        donkeyCock.localScale = origScale;
        SetRandomInput();
    }

    void IncreaseCumLevel()
    {
        cumMultiplier += Random.Range(cumIncreaseRange.x, cumIncreaseRange.y);
        Vector3 newScale =   origScale * cumMultiplier;
        donkeyCock.localScale = new Vector3(donkeyCock.localScale.x, donkeyCock.localScale.y, newScale.z);

        if (cumMultiplier > levelToCumAt)
        {
            TimeToCum();
        }
        else
        {
            SetRandomInput();
        }
    }

    void SetRandomInput()
    {
        int randomArrow = Random.Range(0, possibleInputs.Length);

        currentArrowExpectation = possibleInputs[randomArrow];

        //set correct arrow sprite 
        switch (currentArrowExpectation)
        {
            case KeyCode.UpArrow:
                controlsUIimage.sprite = arrowSprites[0];
                break;
            case KeyCode.DownArrow:
                controlsUIimage.sprite = arrowSprites[1];
                break;
            case KeyCode.RightArrow:
                controlsUIimage.sprite = arrowSprites[2];
                break;
            case KeyCode.LeftArrow:
                controlsUIimage.sprite = arrowSprites[3];
                break;
        }
    }

    //plays cum effects 
    public void TimeToCum()
    {
        cumParticles.Play();

        //change Ui
        controlsUIimage.enabled = false;
        youCameText.enabled = true;

        //reset
        cumResetTime = Random.Range(cumResetRange.x, cumResetRange.y);
        StartCoroutine(WaitToResetDonkeyCock(cumResetTime));
        
        if(donkeySoundFx)
            donkeySoundFx.PlayRandomSoundRandomPitch(donkeySoundFx.donkeyCumSounds, 1f);
    }

    IEnumerator WaitToResetDonkeyCock(float wait)
    {
        yield return new WaitForSeconds(wait);
        
        ResetCumMeter();
        
        cumParticles.Stop();
        controlsUIimage.enabled = true;
        youCameText.enabled = false;
    }
}

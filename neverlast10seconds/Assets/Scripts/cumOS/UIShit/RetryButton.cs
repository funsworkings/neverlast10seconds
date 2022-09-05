using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class RetryButton : MonoBehaviour
{
    public TMP_Text scoreThisTimeText;
    public TMP_Text scoreBestTimeText;

    [SerializeField] private CumMeter _meter;
    
    void Start()
    {
        SetScoreUI();
    }

    private void OnEnable()
    {
        _meter.onPlayerCumEvent.AddListener(CheckHighScore);
    }

    private void OnDisable()
    {
        if(_meter) _meter.onPlayerCumEvent.RemoveListener(CheckHighScore);
    }

    /// <summary>
    /// Retrieve saved high scores. 
    /// </summary>
    public void SetScoreUI()
    {
        scoreThisTimeText.text = "you lasted: " + _meter.scoreTimer;
        scoreBestTimeText.text = "best time: " +  PlayerPrefs.GetFloat("highScore");
    }

    void CheckHighScore()
    {
        //has high score to compare against
        if (PlayerPrefs.HasKey("highScore"))
        {
            float highScore = PlayerPrefs.GetFloat("highScore");
            if (_meter.scoreTimer > highScore)
            {
                SaveHighScore();
            }
        }
        //no high score key
        else
        {
            SaveHighScore();
        }
        
        SetScoreUI();
    }

    /// <summary>
    /// Saves current high score. 
    /// </summary>
    public void SaveHighScore()
    {
        PlayerPrefs.SetFloat("highScore", _meter.scoreTimer);
        SetScoreUI();
    }

    public void ReturntoGame()
    {
        //hook into the main gameplay scene when it's created
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

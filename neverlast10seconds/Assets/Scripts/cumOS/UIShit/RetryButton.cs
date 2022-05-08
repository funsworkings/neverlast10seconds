using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class RetryButton : MonoBehaviour
{
    // Start is called before the first frame update

    public static float scoreThisTime;
    public float scoreBestTime;

    TextMesh scoreThisTimeText;
    TextMesh scoreBestTimeText; 

    void Start()
    {
        //check if this is a high score
        if (scoreThisTime >= scoreBestTime)
        {
            scoreBestTime = scoreThisTime;
        }
        //set the text to the scores
        scoreThisTimeText.text = "you lasted: " + scoreThisTime.ToString();
        scoreBestTimeText.text = "best time: " + scoreBestTime.ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturntoGame()
    {
        //hook into the main gameplay scene when it's created
        SceneManager.LoadScene("ALIENCOMPUTER");
    }
}

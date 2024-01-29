using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //Score increases with time played. Paused when player dead.
    public float score;
    [SerializeField]
    float scoreIncreaseSpeed = 10;

    //If player is dead, pause these.
    public bool scoreAndDifficultyIncreaseDead;

    [SerializeField]
    float startingSpeed;
    [SerializeField]
    float startingTimeBeforeSplitting;

    [SerializeField]
    float finalSpeed; //higher than start speed
    [SerializeField]
    float finalTimeBeforeSplitting; //lower than startingTimeBeforeSplitting

    [SerializeField]
    Text scoreText;
    
    public float currentSpeed;
    public float currentTimeBetweenSplit;

    [SerializeField]
    float scoreWhereDifficultyIsMax;

    private void Start()
    {
        Application.targetFrameRate = -1;
        timeUntilNextUpdate = 0;
    }

    float timeUntilNextUpdate = 10;

    private void Update()
    {
        //Only change these if the player is alive.
        if(scoreAndDifficultyIncreaseDead == false)
        {
            score += Time.deltaTime * scoreIncreaseSpeed;
            timeUntilNextUpdate -= Time.deltaTime;

            scoreText.text = Mathf.FloorToInt(score).ToString();
        }
        else
        {
            scoreText.text = ("Press [P] to play.");
        }

        float lerpValue = Mathf.Clamp01(Mathf.InverseLerp(0, scoreWhereDifficultyIsMax, score));
        currentSpeed = Mathf.Lerp(startingSpeed, finalSpeed, lerpValue);
        currentTimeBetweenSplit = Mathf.Lerp(startingTimeBeforeSplitting, finalTimeBeforeSplitting, lerpValue);

        if(timeUntilNextUpdate < 0)
        {
            timeUntilNextUpdate = 10;
            UpdateSpeedAndSplit();
        }
    }

    //Every frame, update the speed and time between splits. I'd like to do this every frame but because of the
    //excessive amount of GetComponent calls, I'll do it like this instead.
    void UpdateSpeedAndSplit()
    {
        foreach(Transform t in BlockCellsManager.Instance.blockCellsList)
        {
            t.GetComponent<BlockCellMovement>().speed = currentSpeed;
            t.GetComponent<BlockCellSplit>().timerStartingValue = currentTimeBetweenSplit;
        }
    }
}
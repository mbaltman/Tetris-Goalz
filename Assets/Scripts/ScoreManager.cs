using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currScore;
    public int highScore;
    public int interval;

    GameBoardManager gameBoardManager;
    public List<Text> currentScore_Text;
    public List<Text> highScore_Text;
    public List<Text> level_Text;
    private bool updateScore;
    private int deltaScore;
    private int currCombo;
    private int level;
    private int totalLinesCleared;

    void Awake()
    {
      currScore = 0;
      level = 0;
      totalLinesCleared = 0;
      interval = 41;
      updateScore = false;

    }
    void OnEnable()
    {
    }

    void Start()
    {
      highScore = PlayerPrefs.GetInt("highscore", 0);
      Debug.Log("initialhighscore");
      Debug.Log(highScore);
      DisplayText(currentScore_Text, currScore);
      DisplayText(highScore_Text, highScore);
      DisplayText(level_Text, level);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
      DisplayText(level_Text, level);
      if(updateScore)
      {
        currScore += deltaScore;
        deltaScore = 0;
        //currentScore_Text.text = currScore.ToString();
        DisplayText(currentScore_Text, currScore);
        Debug.Log(currScore);
        updateScore = false;
        if(currScore > highScore)
        {
          Debug.Log("NEW HIGHSCORE SET");
          highScore = currScore;
          PlayerPrefs.SetInt ("highscore", highScore);
          DisplayText(highScore_Text, highScore);
        }
      }
    }

    void OnDisable()
    {
        PlayerPrefs.Save();

    }

    public void ScoreLines(int numLines)
    {
      totalLinesCleared += numLines;
      level = (int) (totalLinesCleared / 10);
      interval = 41 - level;


      if(numLines == 0)
      {
        currCombo = 0;
        return;
      }
      else if( numLines == 1)
      {
        deltaScore = 100;
      }
      else if( numLines == 2)
      {
        deltaScore = 300;
      }
      else if( numLines ==3)
      {
        deltaScore = 500;
      }
      else if ( numLines == 4)
      {
        deltaScore = 800;
      }
      //account for combo
      deltaScore = deltaScore + (currCombo * 50 * level);
      currCombo ++;
      updateScore = true;
    }

    public void DisplayText( List<Text> textObjects, int score )
    {
      int scoreHolder = score;
      int multiplier = 10;

      for( int i =0; i< textObjects.Count; i++)
      {
        textObjects[i].text = (scoreHolder%multiplier).ToString();
        scoreHolder = scoreHolder - ( scoreHolder%multiplier);
        multiplier = multiplier *10;
      }

    }
}

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
    private bool tspin;

    void Awake()
    {
      highScore = PlayerPrefs.GetInt("highscore", 0);
      Debug.Log("initialhighscore");
      Debug.Log(highScore);
      DisplayText(highScore_Text, highScore);
      Reset();

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
        if(tspin)
        {
          deltaScore = 400;
          updateScore = true;
          Debug.Log("simplest tspin score");
        }
        return;
      }
      else if( numLines == 1)
      {
        deltaScore = 100;
        if(tspin)
        {
          deltaScore = 800;
        }
      }
      else if( numLines == 2)
      {
        deltaScore = 300;
        if(tspin)
        {
          deltaScore = 1200;
        }
      }
      else if( numLines ==3)
      {
        deltaScore = 500;
        if(tspin)
        {
          deltaScore = 1600;
        }
      }
      else if ( numLines == 4)
      {
        deltaScore = 800;
      }
      //account for combo
      deltaScore = (deltaScore*level) + (currCombo * 50 * level);
      currCombo ++;
      updateScore = true;
      tspin = false;
    }

    public void QuickDrop(int points_delta)
    {
      deltaScore = deltaScore + points_delta;
      updateScore = true;
    }

    public void TSpin()
    {
      tspin = true;
      Debug.Log("TSPIN");
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

   public void Reset()
   {
     currScore = 0;
     level = 0;
     totalLinesCleared = 0;
     interval = 41;
     updateScore = false;

     DisplayText(currentScore_Text, currScore);
     DisplayText(level_Text, level);

   }
}

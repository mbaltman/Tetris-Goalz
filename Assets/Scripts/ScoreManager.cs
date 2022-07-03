using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currScore;
    public int highScore;
    GameBoardManager gameBoardManager;
    Text currentScore_Text;
    Text highScore_Text;
    private bool updateScore;
    private int deltaScore;
    private int currCombo;


    void OnEnable()
    {
        currentScore_Text = GameObject.Find("currentscore_ones").GetComponent<Text>();
        highScore_Text = GameObject.Find("highscore_ones").GetComponent<Text>();
    }

    void Start()
    {
      currScore = 0;
      highScore = PlayerPrefs.GetInt("highscore", 0);
      updateScore = false;
      Debug.Log("initialhighscore ");
      Debug.Log(highScore);
      currentScore_Text.text = currScore.ToString();
      highScore_Text.text = highScore.ToString();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
      if(updateScore)
      {
        currScore += deltaScore;
        deltaScore = 0;
        currentScore_Text.text = currScore.ToString();

        Debug.Log(currScore);
        updateScore = false;
        if(currScore > highScore)
        {
          Debug.Log("NEW HIGHSCORE SET");
          highScore = currScore;
          PlayerPrefs.SetInt ("highscore", highScore);
          highScore_Text.text = highScore.ToString();

        }
      }
    }


    void OnDisable()
    {
        PlayerPrefs.Save();

    }

    public void ScoreLines(int numLines, int level)
    {
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
}

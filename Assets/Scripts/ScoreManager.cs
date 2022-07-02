using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public int highScore;
    GameBoardManager gameBoardManager;
    Text currentScore_Text;
    Text highScore_Text;
    private bool updateScore;
    private int deltaScore;


    void OnEnable()
    {
        gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
        gameBoardManager.ScoreLines += ScoreLines;
        currentScore_Text = GameObject.Find("currentscore_ones").GetComponent<Text>();
        highScore_Text = GameObject.Find("highscore_ones").GetComponent<Text>();
    }

    void Start()
    {
      currentScore = 0;
      highScore = PlayerPrefs.GetInt("highscore", 0);
      updateScore = false;
      Debug.Log("initialhighscore ");
      Debug.Log(highScore);
      currentScore_Text.text = currentScore.ToString();
      highScore_Text.text = highScore.ToString();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
      if(updateScore)
      {
        currentScore += deltaScore;
        deltaScore = 0;
        currentScore_Text.text = currentScore.ToString();

        Debug.Log(currentScore);
        updateScore = false;
        if(currentScore > highScore)
        {
          Debug.Log("NEW HIGHSCORE SET");
          highScore = currentScore;
          PlayerPrefs.SetInt ("highscore", highScore);
          highScore_Text.text = highScore.ToString();

        }
      }
    }


    void OnDisable()
    {
        gameBoardManager.ScoreLines -= ScoreLines;
        PlayerPrefs.Save();

    }

    public void ScoreLines(int numLines)
    {
      deltaScore = numLines;
      updateScore = true;
    }
}

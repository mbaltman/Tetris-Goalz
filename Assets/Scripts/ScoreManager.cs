using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public int highScore;
    GameBoardManager gameBoardManager;
    private bool updateScore;
    private int deltaScore;

    void Start()
    {
      currentScore = 0;
      highScore = PlayerPrefs.GetInt ("highscore", highScore);
      updateScore = false;
    }

    void OnEnable()
    {
        gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
      //  gameBoardManager.ScoreLines += ScoreLines;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      if(updateScore)
      {
        currentScore += deltaScore;
        deltaScore = 0;
        Debug.Log(currentScore);
        updateScore = false;
      }
    }


    void OnDisable()
    {
      //  gameBoardManager.ScoreLines -= ScoreLines;
    }

    public void ScoreLines(int numLines)
    {
      deltaScore = numLines;
      updateScore = true;
    }
}

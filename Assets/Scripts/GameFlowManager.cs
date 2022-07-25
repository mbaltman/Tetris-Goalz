using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

using static Constants;
using Random = System.Random;
using Object = UnityEngine.Object;

/*
This class manages the flow of the piece over the course of the game,
pieces are create and destroyed
*/

public class GameFlowManager : MonoBehaviour
{

    // objects instantiaded within this script.
    private GameObject activePieceInstance;
    private ActivePieceController controller;
    private ScoreManager scoreManager;
    private GameBoardManager gameBoardManager;

    public GameObject [] activePiecePrefabs;
    public GameObject menuCanvas;
    private int [] randomIndexList = { 0,1,2,3,4,5,6};
    private int currRandomIndex;
    private bool canHold;

    //these indexes are the ones for all of the arrays in CONSTANT, that describe each individual tetriminio
    private int nextPieceIndex;
    private int holdPieceIndex;
    private int currPieceIndex;

    public delegate void UpdateSignDelegate(int index, string currPiece);
    public event UpdateSignDelegate UpdatePiece;

    public delegate void ResetSignDelegate();
    public event ResetSignDelegate VoidSign;

    private gameState currentGameState;


    void Awake()
    {
      currRandomIndex = 7;
      holdPieceIndex = -1;
      currPieceIndex = GetRandomPiece();
      nextPieceIndex = GetRandomPiece();

      scoreManager = GameObject.Find("ScoreSign").GetComponent<ScoreManager>();
      gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
      currentGameState = gameState.NewGame;
    }

    void Start()
    {
      Time.timeScale = 0;
    }

    /*
    function to generate a new piece, using CurrPieceIndex
    */
    private void CreatePiece()
    {
      activePieceInstance = Instantiate(activePiecePrefabs[currPieceIndex]);
      controller = activePieceInstance.GetComponent<ActivePieceController>();
      Debug.Log("SCORE MANAGER INTERVAL " + scoreManager.interval);
      controller.Setup(currPieceIndex, scoreManager.interval);
      controller.OnHitBottom += StopPiece;
      controller.OnHold += HoldPiece;

      gameBoardManager.GameOver += EndGame;
    }
/*
function to destroy active piece, once it stops.
Whenever a piece stops, immediately create a new one.
*/
    public void StopPiece()
    {
      int linesCleared = controller.linesCleared;
      Debug.Log( "On Stop event Recieved");
      if(controller.tspin)
      {
        scoreManager.TSpin();
      }
      scoreManager.ScoreLines(linesCleared);

      RemovePiece();
      if(linesCleared >0)
      {
        float timeWait = Time.time + Constants.lineClearTime;

        Debug.Log("WAIT TO MAKE NEXT PIECE");
      }
      currPieceIndex =nextPieceIndex;
      nextPieceIndex = GetRandomPiece();
      if(UpdatePiece != null)
      {
        UpdatePiece(nextPieceIndex, "NextPiece");
      }
      canHold = true;
      CreatePiece();

    }

    public void RemovePiece()
    {
      Debug.Log("REMOVE PIECE");
      controller.enabled=false;
      controller.OnHitBottom -= StopPiece;
      Destroy(activePieceInstance);

    }


    private int GetRandomPiece()
    {
      int returnVal = 0;
      if(currRandomIndex == 7)
      {
            Random random = new Random();
            randomIndexList = randomIndexList.OrderBy(x => random.Next()).ToArray();
            currRandomIndex = 0;
      }
      returnVal = randomIndexList[currRandomIndex];
      currRandomIndex++;
      return returnVal;

    }

    private void HoldPiece()
    {
      if( canHold == true)
      {
        canHold = false;
        RemovePiece();
        //need to add a check for if holdPieceIndex is null
        if(holdPieceIndex != -1)
        {
          int temp = holdPieceIndex;
          holdPieceIndex = currPieceIndex;
          currPieceIndex = temp;
          UpdatePiece(holdPieceIndex, "HoldPiece");
        }
        else
        {
          holdPieceIndex = currPieceIndex;
          currPieceIndex = nextPieceIndex;
          nextPieceIndex = GetRandomPiece();
          UpdatePiece(holdPieceIndex, "HoldPiece");
          UpdatePiece(nextPieceIndex, "NextPiece");
        }

        CreatePiece();

      }
    }
    private void EndGame()
    {
      Debug.Log("ENDGAME");
      PauseGame();
      currentGameState = gameState.Ended;


    }

    public void StartGame()
    {
      //start game

      //hide menu
      gameObject.GetComponent<Renderer>().enabled = false;
      menuCanvas.SetActive(false);
      //if needed create new piece
      if(currentGameState == gameState.Ended)
      {
        Debug.Log("GOING TO RESET GAME");
        ResetGame();
      }
      if(currentGameState == gameState.NewGame)
      {
        CreatePiece();
        if(UpdatePiece != null)
        {
          UpdatePiece(nextPieceIndex, "NextPiece");
          canHold = true;
        }
      }
      //display hold and next piece
      UpdatePiece(holdPieceIndex, "HoldPiece");
      UpdatePiece(nextPieceIndex, "NextPiece");
      Camera.main.cullingMask = -1;

      currentGameState = gameState.Playing;
      Time.timeScale = 1;
    }

    public void PauseGame()
    {
      //stop game
      Time.timeScale = 0;
      //show sign
      gameObject.GetComponent<Renderer>().enabled = true;
      menuCanvas.SetActive(true);

      //hides next and hold pieces
      VoidSign();
      Camera.main.cullingMask = LayerMask.GetMask("UI", "Background");

      currentGameState = gameState.Paused;
    }

    public void ResetGame()
    {
      Debug.Log("RESETTING GAME");
      Time.timeScale = 0;
      RemovePiece();
      //resets randomizer
      currRandomIndex = 7;
      //reset SCORE
      scoreManager.Reset();
      //reset hold and next piece displays
      if( VoidSign != null)
      {
        holdPieceIndex = -1;
        nextPieceIndex = -1;
        UpdatePiece(holdPieceIndex, "HoldPiece");
        UpdatePiece(nextPieceIndex, "NextPiece");
      }
      //reset Board
      gameBoardManager.Reset();

      currentGameState = gameState.NewGame;
    }
  }

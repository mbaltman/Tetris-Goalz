using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Security.Cryptography;

using static Constants;
using Random = System.Random;
using Object = UnityEngine.Object;


/*
This class manages the flow of the piece over the course of the game,
pieces are create and destroyed
*/

public class GameFlowManager : MonoBehaviour
{


    private GameObject activePieceInstance;
    private ActivePieceController controller;
    public GameObject [] activePiecePrefabs;
    private int [] randomIndexList = { 0,1,2,3,4,5,6};
    private int currRandomIndex;
    private bool canHold;

    //these indexes are the ones for all of the arrays in CONSTANT, that describe each individual tetriminio
    private int nextPieceIndex;
    private int holdPieceIndex;
    private int currPieceIndex;

    public delegate void ActivePieceDelegate(int index, string currPiece);
    public event ActivePieceDelegate UpdatePiece;


    void Awake()
    {
      currRandomIndex = 7;
      holdPieceIndex = -1;
      currPieceIndex = GetRandomPiece();
      nextPieceIndex = GetRandomPiece();
      CreatePiece();
    }

    void Start()
    {
      if(UpdatePiece != null)
      {
        UpdatePiece(nextPieceIndex, "NextPiece");
        canHold = true;
      }
    }

    /*
    function to generate a new piece, using CurrPieceIndex
    */
    private void CreatePiece()
    {
      activePieceInstance = Instantiate(activePiecePrefabs[currPieceIndex]);
      controller = activePieceInstance.GetComponent<ActivePieceController>();
      controller.Setup(Constants.tetriminoMatrices[currPieceIndex], currPieceIndex);
      controller.OnHitBottom += StopPiece;
      controller.OnHold += HoldPiece;
      GameObject.Find("Grid").GetComponent<GameBoardManager>().GameOver += EndGame;
    }
/*
function to destroy active piece, once it stops.
Whenever a piece stops, immediately create a new one.
*/
    public void StopPiece()
    {
      Debug.Log( "On Stop event Recieved");

      RemovePiece();
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
      controller.OnDisable();
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
      controller.enabled=false;
    }
  }

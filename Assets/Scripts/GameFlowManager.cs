using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Security.Cryptography;

using static Constants;
using Random = System.Random;


/*
This class manages the flow of the piece over the course of the game,
pieces are create and destroyed
*/

public class GameFlowManager : MonoBehaviour
{


    private GameObject activePieceInstance;
    private ActivePieceController controller;
    private int [] randomIndexList = { 0,1,2,3,4,5,6};
    private int currRandomIndex;


    void Awake()
    {
      CreatePiece();
      currRandomIndex = 7;
    }

    void OnEnable()
    {

    }




    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {

    }

    /*
    function to generate a new piece, and set it up with a randomly determined tetrimino
    */
    private void CreatePiece()
    {
      int index = GetRandomPiece();
      activePieceInstance = (GameObject)AssetDatabase.LoadAssetAtPath(Constants.tetriminoPrefabs[index], typeof(GameObject)) ;
      activePieceInstance = Instantiate(activePieceInstance);
      controller = activePieceInstance.GetComponent<ActivePieceController>();
      controller.Setup(Constants.tetriminoMatrices[index], index);
      controller.OnStop += StopPiece;
    }
/*
function to destroy active piece, once it stops.
Whenever a piece stops, immediately create a new one.
*/
    public void StopPiece()
    {
      Debug.Log( "On Stop event Recieved");
      controller.enabled=false;
      controller.OnStop -= StopPiece;
      controller.OnDisable();
      Destroy(activePieceInstance);
      CreatePiece();

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
  }

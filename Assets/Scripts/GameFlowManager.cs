using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Constants;


/*
This class manages the flow of the piece over the course of the game,
pieces are create and destroyed
*/

public class GameFlowManager : MonoBehaviour
{


    private GameObject activePieceInstance;
    private ActivePieceController controller;


    void Awake()
    {
      CreatePiece();
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
      int index = (int) Mathf.Floor(Random.Range(0,7));

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
  }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;
using static Matrix;
using static GameBoardManager;

public class ActivePieceController : MonoBehaviour, PlayerControls.IGameplayActions

{
    public PlayerControls userInput;
    public float startX;
    public float startY;

    public int interval;
    private float stoppedTime;
    private float lowestPoint;
    private bool moveLeft_b = false;
    private bool moveRight_b = false;
    private bool rotateRight_b = false;
    private bool rotateLeft_b = false;
    private bool hardDrop_b = false;
    private int counter = 0;
    private int index;
    InputAction action;
    public Matrix pieceMatrix;
    GameBoardManager gameBoardManager;


    public delegate void ActivePieceDelegate();
    public event ActivePieceDelegate OnHitBottom;
    public event ActivePieceDelegate OnHold;

    public void Setup(int [,] currPieceMatrix, int currIndex)
    {
      pieceMatrix = new Matrix(currPieceMatrix);
      index = currIndex;
    }

    public void OnEnable()
    {
      if(userInput == null)
      {
        userInput = new PlayerControls();

        userInput.gameplay.SetCallbacks(this);
      }
      userInput.gameplay.Enable();
      gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
      transform.position = new Vector3(startX,startY,0f);
      lowestPoint = transform.position.y;
    }

    public void OnDisable()
    {
      userInput.gameplay.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      if(moveLeft_b)
      {
        if(gameBoardManager.CheckMoveLeft(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(-1f,0f,0f);
        }
        moveLeft_b = false;
      }

      if(moveRight_b)
      {
        if(gameBoardManager.CheckMoveRight(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(1f,0f,0f);
        }
        moveRight_b = false;
      }

      if(rotateLeft_b)
      {
        if(gameBoardManager.CheckRotationLeft(transform.position, pieceMatrix))
        {
          transform.Rotate(0,0,90);
          pieceMatrix.RotateLeft();
        }
        rotateLeft_b = false;
      }

      if(rotateRight_b)
      {
        if(gameBoardManager.CheckRotationRight(transform.position, pieceMatrix))
        {
          transform.Rotate(0,0,-90);
          pieceMatrix.RotateRight();
        }
        rotateRight_b = false;
      }

      if(hardDrop_b)
      {
        while(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
        }
        hardDrop_b = false;

      }

      if( counter ==interval)
      {
        if(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
        }
          counter = 0;
      }



      counter++;
      CheckLockDelay();
    }

    public void OnMoveRight(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        moveRight_b = true;
      }
    }

    public void OnMoveLeft(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        moveLeft_b = true;
      }
    }

    public void OnRotateLeft(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        rotateLeft_b = true;
      }
    }
    public void OnRotateRight(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        rotateRight_b = true;
      }
    }
    public void OnHoldPiece(InputAction.CallbackContext value)
    {
      if(OnHold != null)
      {
        OnHold();
      }
    }

    public void OnHardDrop(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        hardDrop_b = true;
        stoppedTime = Constants.lockTime + 0.1f;

      }
    }

    public void OnFastDrop(InputAction.CallbackContext value)
    {
      if(value.started)
      {
        counter = 0; 
        Debug.Log("started Fast Drop");
        interval  = interval /2;
      }
      if(value.canceled)
      {
        Debug.Log("stopped Fast Drop");
        interval  = interval *2;
      }
      Debug.Log("curr interval " + interval);
    }

    //check if piece has stopped moving long enough, that it should lock into place.
    private void CheckLockDelay()
    {


      //if this is true, then the piece has stopped moving, and reached the end of its life cycle
      if(stoppedTime > Constants.lockTime )
      {
        if(OnHitBottom != null)
        {
          if(gameBoardManager.SavePieceToBackground(transform.position, pieceMatrix, index) == 1)
          {
            gameBoardManager.CheckForClearedLines(transform.position, pieceMatrix);
            OnHitBottom();
          }
        }
      }


      if(transform.position.y < lowestPoint)
      {
        lowestPoint = transform.position.y;
        stoppedTime =0f;
      }
      else
      {
        stoppedTime += Time.fixedDeltaTime;
      }
    }
}

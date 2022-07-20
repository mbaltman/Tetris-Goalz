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
    public int linesCleared;
    public int interval;
    public int state;
    public bool tspin;
    //tracks current position , 0 = spawn, 1 = right of spawn, 2 = 180 of spawn, 3 = left of spawn

    private float stoppedTime;
    private float lowestPoint;
    private bool moveLeft_b = false;
    private bool holdLeft_b = false;
    private bool holdRight_b = false;
    private bool moveRight_b = false;
    private bool rotateRight_b = false;
    private bool rotateLeft_b = false;
    private bool hardDrop_b = false;
    private int counter = 0;
    private int index;
    private bool fastDropActive;
    private int points_delta;
    private InputAction action;
    public Matrix pieceMatrix;
    private GameBoardManager gameBoardManager;
    private ScoreManager scoreManager;


    public delegate void ActivePieceDelegate();
    public event ActivePieceDelegate OnHitBottom;
    public event ActivePieceDelegate OnHold;

    public void Setup( int currIndex, int currInterval)
    {
      tspin = false;
      pieceMatrix = new Matrix(currIndex);
      index = currIndex;
      linesCleared = 0;
      interval = currInterval;
    }

    void OnEnable()
    {
      if(userInput == null)
      {
        userInput = new PlayerControls();

        userInput.gameplay.SetCallbacks(this);
      }
      Debug.Log("Enable Piece");
      userInput.gameplay.Enable();
      gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
      scoreManager = GameObject.Find("ScoreSign").GetComponent<ScoreManager>();
      transform.position = new Vector3(startX,startY,0f);
      lowestPoint = transform.position.y;
      fastDropActive = false;
      state =0;
    }

    public void OnDisable()
    {
      Debug.Log("Disable Piece");
      userInput.gameplay.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      points_delta = 0;
      int nextState = 0;
      List<Vector2> kickbacks = new List<Vector2>();
      int kickbackIndex = -1;

      if(moveLeft_b || (holdLeft_b && (counter%10 == 0)))
      {
        if(gameBoardManager.CheckMoveLeft(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(-1f,0f,0f);
        }
        moveLeft_b = false;
      }

      if(moveRight_b || (holdRight_b && (counter%10 == 0)))
      {
        if(gameBoardManager.CheckMoveRight(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(1f,0f,0f);
        }
        moveRight_b = false;
      }

      if(rotateLeft_b)
      {
        nextState = NextState(state, "left");
        kickbacks = GetKickbacks(state, nextState);
        if((kickbackIndex = gameBoardManager.CheckRotationLeft(transform.position, pieceMatrix, kickbacks)) != -1 )
        {
          transform.Rotate(0,0,90);
          pieceMatrix.RotateLeft();
          transform.position = transform.position + new Vector3(kickbacks[kickbackIndex].x,kickbacks[kickbackIndex].y,0f);
          state = nextState;
          Debug.Log("newState: " + state);
        }
        rotateLeft_b = false;
      }

      if(rotateRight_b)
      {
        nextState = NextState(state, "right");
        kickbacks = GetKickbacks(state, nextState);
        if((kickbackIndex = gameBoardManager.CheckRotationLeft(transform.position, pieceMatrix, kickbacks)) != -1 )
        {
          transform.Rotate(0,0,-90);
          pieceMatrix.RotateRight();
          transform.position = transform.position + new Vector3(kickbacks[kickbackIndex].x,kickbacks[kickbackIndex].y,0f);
          state = nextState;
          Debug.Log("newState: " + state);
        }
        rotateRight_b = false;

      }

      if(hardDrop_b)
      {
        while(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
          points_delta = points_delta + 2;
        }
        hardDrop_b = false;

      }

      if( counter >=interval || ( fastDropActive && counter >= 10))
      {
        if(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
          if(fastDropActive )
          {
            points_delta = points_delta + 1;
          }
        }
          counter = 0;
      }

      scoreManager.QuickDrop(points_delta);
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
        fastDropActive = true;
        Debug.Log("started Fast Drop");
      }

      if(value.canceled)
      {
        Debug.Log("stopped Fast Drop");
        fastDropActive = false;
      }
      Debug.Log("curr interval " + interval);
    }

    public void OnHoldLeft(InputAction.CallbackContext value)
    {
      if(value.performed)
      {
        holdLeft_b = true;
        Debug.Log("HOLDING LEFT");

      }
      if(value.canceled)
      {
        holdLeft_b = false;
        Debug.Log("HOLDING LEFT Canceled");
      }
    }

    public void OnHoldRight(InputAction.CallbackContext value)
    {
      if(value.performed)
      {
        holdRight_b = true;
        Debug.Log("HOLDING Right");

      }
      if(value.canceled)
      {
        holdRight_b = false;
        Debug.Log("HOLDING Right Canceled");
      }
    }
    //check if piece has stopped moving long enough, that it should lock into place.
    private void CheckLockDelay()
    {
      //stopped time gets reset, if the game is paused.
      if(Time.timeScale == 0)
      {
        stoppedTime =0;
      }
      //if this is true, then the piece has stopped moving, and reached the end of its life cycle
      if(stoppedTime > Constants.lockTime )
      {
        //snaps piece to bottom, in case kickbacks moved it up.
        while(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
        }

        if(OnHitBottom != null)
        {
          if(gameBoardManager.SavePieceToBackground(transform.position, pieceMatrix, index) == 1)
          {
            Debug.Log("SAVED TO BACKGROUND");
            tspin = gameBoardManager.CheckTSpin(transform.position, pieceMatrix, index);
            linesCleared = gameBoardManager.CheckForClearedLines(transform.position, pieceMatrix);
            Debug.Log("LINES CLEARED: " + linesCleared );
            Debug.Log("Hit the Bottom");
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

    /* used to calculate next state, returns -1 if given invalid input */
    int NextState( int currState, string turn)
    {
      int nextState;
      if( turn == "left")
      {
        nextState = currState -1;
      }
      else if( turn == "right")
      {
        nextState = currState +1;
      }
      else
      {
        Debug.Log("INVALID INPUT");
        return -1;
      }

      if(nextState == -1)
      {
        nextState = 3;
      }
      else if(nextState == 4)
      {
        nextState = 0;
      }

      return nextState;
    }

    List<Vector2> GetKickbacks( int currState, int nextState)
    {
      List<Vector2> kickbacks = new List<Vector2>();
      kickbacks.Add( new Vector2(0,0));
      int mappedVal = (currState * 10) + nextState;
      //corresponds to Ipiece
      if( index == 0 )
      {
        switch(mappedVal)
        {
          case 1:
          case 32:
            kickbacks.Add( new Vector2(-2,0));
            kickbacks.Add( new Vector2(1,0));
            kickbacks.Add( new Vector2(-2,-1));
            kickbacks.Add( new Vector2(1,2));
            break;
          case 10:
          case 23:
            kickbacks.Add( new Vector2(2,0));
            kickbacks.Add( new Vector2(-1,0));
            kickbacks.Add( new Vector2(2,1));
            kickbacks.Add( new Vector2(-1,-2));
            break;
          case 12:
          case 3:
            kickbacks.Add( new Vector2(-1,0));
            kickbacks.Add( new Vector2(2,0));
            kickbacks.Add( new Vector2(-1,2));
            kickbacks.Add( new Vector2(2,-1));
            break;
          case 21:
          case 30:
            kickbacks.Add( new Vector2(1,0));
            kickbacks.Add( new Vector2(-2,0));
            kickbacks.Add( new Vector2(1,-2));
            kickbacks.Add( new Vector2(-2,1));
            break;

        }

      }
      else
      {

        switch(mappedVal)
        {
          case 1:
          case 21:
            kickbacks.Add( new Vector2(-1,0));
            kickbacks.Add( new Vector2(-1,1));
            kickbacks.Add( new Vector2(0,-2));
            kickbacks.Add( new Vector2(-1,-2));
            break;
          case 10:
          case 12:
            kickbacks.Add( new Vector2(1,0));
            kickbacks.Add( new Vector2(1,-1));
            kickbacks.Add( new Vector2(0,2));
            kickbacks.Add( new Vector2(1,2));
            break;
          case 23:
          case 3:
            kickbacks.Add( new Vector2(1,0));
            kickbacks.Add( new Vector2(1,1));
            kickbacks.Add( new Vector2(0,-2));
            kickbacks.Add( new Vector2(1,-2));
            break;
          case 30:
          case 32:
            kickbacks.Add( new Vector2(-1,0));
            kickbacks.Add( new Vector2(-1,-1));
            kickbacks.Add( new Vector2(0,2));
            kickbacks.Add( new Vector2(-1,2));
            break;
        }

      }
      return kickbacks;
    }

}

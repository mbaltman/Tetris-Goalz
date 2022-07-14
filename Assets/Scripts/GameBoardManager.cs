using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Matrix;
using static Constants;
/*
This class maintains the board.
It checks for all boundary conditions and manages when tiles get cleared/ move.
It does not do any of the actual movement, it just monitors and reports on the state of the board.
*/
public class GameBoardManager : MonoBehaviour
{
    public Sprite [] tileSpries;
    public int [,] availabilityGrid;
    private GameObject currTile;

    public delegate void ClearLineDelegate(int row);
    public event ClearLineDelegate ClearLine;
    public GameObject backgroundTilePlaceholder;

    public delegate void GameOverDelegate();
    public event GameOverDelegate GameOver;



    void Awake()
    {
      availabilityGrid =  Constants.initialGameBoard.Clone() as int[,];

    }


    // returns -1 if impossible and returns index into kickback list if it worked
    public int CheckRotationLeft(Vector3 position, Matrix pieceMatrix, List<Vector2> kickbacks)
    {
      Debug.Log("checking rotation left");
      Matrix rotated = new Matrix(pieceMatrix);
      rotated.RotateLeft();
      for ( int i = 0;  i< kickbacks.Count; i++)
      {
        Debug.Log("index: " + i);
        Vector2 currKickback = kickbacks[i];
        if(CheckAvailability(new Vector3(position.x + currKickback.x,position.y + currKickback.y,0), rotated))
          return i;
      }
      return -1;
    }
    public int CheckRotationRight(Vector3 position, Matrix pieceMatrix, List<Vector2> kickbacks)
    {
      Matrix rotated = new Matrix(pieceMatrix);
      rotated.RotateRight();
      for ( int i = 0;  i< kickbacks.Count; i++)
      {
        Debug.Log("index: " + i);
        Vector2 currKickback = kickbacks[i];
        if(CheckAvailability(new Vector3(position.x + currKickback.x,position.y + currKickback.y,0), rotated))
          return i;
      }
      return -1;
    }

    public bool CheckMoveDown(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 position_copy = new Vector3(position.x, position.y, position.z);
      position_copy.y = position.y -1f;
      return CheckAvailability(position_copy, pieceMatrix);
    }

    public bool CheckMoveLeft(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 position_copy = new Vector3(position.x, position.y, position.z);
      position_copy.x = position.x -1f;
      return CheckAvailability(position_copy, pieceMatrix);
    }

    public bool CheckMoveRight(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 position_copy = new Vector3(position.x, position.y, position.z);
      position_copy.x = position.x +1f;
      return CheckAvailability(position_copy, pieceMatrix);

    }
    //This will check if a given position/pieceMatrix is possible.
    private bool CheckAvailability(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 matrixPosition = pieceMatrix.GetCenter();
      int [,] matrix = pieceMatrix.GetMatrix();
      //string msg;
      Vector3 currPosition = new Vector3(0f,0f,0f);
      //scan to see if rotated Matrix overlaps with anything,
      for (int row = 0; row < Constants.tetriminoHeight; row++)
      {
        for (int col = 0; col <  Constants.tetriminoWidth; col++)
        {
          currPosition.x = (position.x  + col) - matrixPosition.x;
          currPosition.y = (position.y  + row) - matrixPosition.y;

          if(matrix[col,row] > 0)
          {
            if( currPosition.x < 0 || currPosition.x >Constants.boardWidth || currPosition.y <0 || currPosition.y > (Constants.boardHeight + Constants.boardExtraHeight) )
            {
              return false;
            }
            else if (availabilityGrid[(int)currPosition.x, (int)currPosition.y] >0 )
            {
              //msg = "conflict found at point x " + currPosition.x + "y " + currPosition.y;
              //Debug.Log(msg);
              return false;
            }
          }
        }
     }
      return true;

    }


    /*
    CheckForClearedLines scans the four lines that may have been cleared by the most recent piece dropping
    and if necessary, invokes the function to remove those lines.

    returns number of lines cleared
    */
    public int CheckForClearedLines(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 matrixPosition = pieceMatrix.GetCenter();
      Vector3 currPosition = new Vector3(0f,0f,0f);
      bool complete = false;
      //string msg = "";
      List<int> linesToClear = new List<int>();
      int numLines = 0;

      //adjust highest y line
      currPosition.y = position.y  - matrixPosition.y;
      if(currPosition.y < 0)
      {
        currPosition.y = 0;
      }
      Debug.Log("curr position y : " + currPosition.y);

      for ( int row =0; row < Constants.tetriminoHeight; row ++ )
      {
        complete = true;
      //  msg ="complete line";
        for( int col = 0; col < Constants.boardWidth; col ++)
        {
          if(availabilityGrid[col, row+(int)currPosition.y] == 0)
          {
            complete = false;
          //  msg = "incomplete line";
            break;
          }
        }
        if(complete)
        {
          if(ClearLine != null)
          {
            //Debug.Log("clear line: "+ (row+(int)currPosition.y) );
            ClearLine(row+(int)currPosition.y);
            linesToClear.Add(row+(int)currPosition.y);
            numLines++;
          }
        }

        //Debug.Log(msg);

      }
      int offset = 0;
      foreach( int row in linesToClear)
      {
        DeleteRow(row-offset);
        offset++;
      }
      return numLines;
    }

    public void DeleteRow(int rowCleared)
    {
      for (int row = 0; row < Constants.boardHeight + Constants.boardExtraHeight; row++)
      {
        for (int col = 0; col <  Constants.boardWidth; col++)
        {
          if(row> rowCleared )
          {
            //shift everything down 1 in the y direction
            availabilityGrid[col,row-1] = availabilityGrid[col,row];
          }
        }
      }

    }
    /*
    when a piece stops moving, it invokes this method. This allows the piece to be saved to the background
    and be marked off in the availabilityGrid
    retruns 1 when piece saves to background successfully
    returns -1 when game ends
    */
    public int SavePieceToBackground(Vector3 position, Matrix pieceMatrix, int index)
    {
      Debug.Log("SavePieceToBackground");
      int [,] matrix = pieceMatrix.GetMatrix();
      Vector3 matrixPosition = pieceMatrix.GetCenter();
      Vector3 currPosition = new Vector3(0f,0f,0f);

      for (int row = 0; row < Constants.tetriminoHeight; row++)
      {
        for (int col = 0; col <  Constants.tetriminoWidth; col++)
        {
          if(matrix[col,row] > 0)
          {
            currPosition.x = (position.x  + col) - matrixPosition.x;
            currPosition.y = (position.y  + row) - matrixPosition.y;
            if(currPosition.y> Constants.boardHeight)
            {
              Debug.Log("GAMEOVER");
              GameOver();
              return -1;
            }

            availabilityGrid[(int)currPosition.x,(int)currPosition.y ] = 1;

            currTile = Instantiate(backgroundTilePlaceholder, currPosition, Quaternion.identity);
            currTile.GetComponent<SpriteRenderer>().sprite = tileSpries[index];
          }
        }
      }
      return 1;

    }

    public void Reset()
    {
      availabilityGrid =  Constants.initialGameBoard.Clone() as int[,];

      for( int i =0; i< Constants.boardHeight; i ++ )
      {
        ClearLine(i);
      }
    }

}

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
    public int [,] availabilityGrid;
    private GameObject backgroundTilePlaceholder;

    public delegate void GameBoardDelegate(int row);
    public event GameBoardDelegate ClearLine;

    // Start is called before the first frame update
    void Start()
    {
      availabilityGrid = Constants.initialGameBoard;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckRotationLeft(Vector3 position, Matrix pieceMatrix )
    {
      Matrix rotated = new Matrix(pieceMatrix);
      rotated.RotateLeft();
      return CheckAvailability(position, rotated);
    }
    public bool CheckRotationRight(Vector3 position, Matrix pieceMatrix)
    {
      Matrix rotated = new Matrix(pieceMatrix);
      rotated.RotateRight();
      return CheckAvailability(position, rotated);
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
      string msg;
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
            if( currPosition.x < 0 || currPosition.x >Constants.boardWidth || currPosition.y <0 || currPosition.y > Constants.boardHeight + Constants.boardExtraHeight )
            {
              msg = "conflict found at point x " + currPosition.x + "y " + currPosition.y;
              Debug.Log(msg);
              return false;
            }
            else if (availabilityGrid[(int)currPosition.x, (int)currPosition.y] >0 )
            {
              msg = "conflict found at point x " + currPosition.x + "y " + currPosition.y;
              Debug.Log(msg);
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
    */
    public void CheckForClearedLines(Vector3 position, Matrix pieceMatrix)
    {
      Vector3 matrixPosition = pieceMatrix.GetCenter();
      Vector3 currPosition = new Vector3(0f,0f,0f);
      bool complete = false;
      string msg = "";
      List<int> linesToClear = new List<int>();

      //adjust highest y line
      currPosition.y = position.y  - matrixPosition.y;

      for ( int row =0; row < Constants.tetriminoHeight; row ++ )
      {
        complete = true;
        msg ="complete line";
        for( int col = 0; col < Constants.boardWidth; col ++)
        {
          if(availabilityGrid[col, row+(int)currPosition.y] == 0)
          {
            complete = false;
            msg = "incomplete line";
            break;
          }
        }
        if(complete)
        {
          if(ClearLine != null)
          {
            Debug.Log("clear line: "+ (row+(int)currPosition.y) );
            ClearLine(row+(int)currPosition.y);
            linesToClear.Add(row+(int)currPosition.y);
          }
        }
        Debug.Log(msg);

      }

      int offset = 0;
      foreach( int row in linesToClear)
      {
        DeleteRow(row-offset);
        offset++;
      }
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
    */
    public void SavePieceToBackground(Vector3 position, Matrix pieceMatrix, int index)
    {
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

            availabilityGrid[(int)currPosition.x,(int)currPosition.y ] = 1;
            backgroundTilePlaceholder = (GameObject)AssetDatabase.LoadAssetAtPath(Constants.tilePrefabs[index], typeof(GameObject)) ;
            Instantiate(backgroundTilePlaceholder, currPosition, Quaternion.identity);
          }
        }
      }

    }

}

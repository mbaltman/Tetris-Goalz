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
            if( currPosition.x < 0 || currPosition.x >10 || currPosition.y <0 || currPosition.y > 23 )
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


    public void CheckForClearedLines(Vector3 position, Matrix pieceMatrix)
    {

    }
    public void RemoveClearedLines()
    {

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

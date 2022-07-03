using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

/* this class creates accesses the Matrices that are used to represent the tetris pieces in 3d space */
/*
Reference for RotateLeft and RotateRight
https://www.cyotek.com/blog/rotating-an-array-using-csharp
*/

public class Matrix
{
    private int [,] matrix_a;
    private int pieceIndex;

    public Matrix( int index)
    {
      if(index >= 0 && index < 7 )
      {
        matrix_a = Constants.tetriminoMatrices[index];
        pieceIndex = index;
      }
      else
      {
        Debug.Log("Incorrect size input to Matrix Constructor");
      }
    }
    public Matrix(Matrix original)
    {
      matrix_a = original.GetMatrix();
    }
    public void RotateLeft()
    {
      int[,] dst = new int[Constants.tetriminoHeight, Constants.tetriminoWidth];
      for (int row = 0; row < Constants.tetriminoHeight; row++)
      {
        for (int col = 0; col <  Constants.tetriminoWidth; col++)
        {
            int newRow;
            int newCol;

            newRow = col;
            newCol = Constants.tetriminoHeight - (row + 1);

            dst[newCol, newRow] = matrix_a[col, row];
        }
      }
      matrix_a = dst;
      /* rotate left */
    }
    public void RotateRight()
    {
      /* rotate right */
      int[,] dst = new int[Constants.tetriminoHeight,  Constants.tetriminoWidth];
      for (int row = 0; row < Constants.tetriminoHeight; row++)
      {
        for (int col = 0; col <  Constants.tetriminoWidth; col++)
        {
            int newRow;
            int newCol;

            newRow =  Constants.tetriminoWidth - (col + 1);
            newCol = row;

            dst[newCol, newRow] = matrix_a[col, row];
        }
      }
      matrix_a = dst;
    }
    public int [,] GetMatrix()
    {
      return matrix_a;
    }

    public Vector3 GetCenter()
    {
      //defaults to center
      Vector3 returnVal = new Vector3(1.5f,1.5f,1.5f);
      for (int row = 0; row < Constants.tetriminoHeight; row++)
      {
        for (int col = 0; col <  Constants.tetriminoWidth; col++)
        {
            if(matrix_a[col,row] ==2)
            {
              returnVal.x = col;
              returnVal.y = row;

            }
        }

      }
    return returnVal;
  }

}

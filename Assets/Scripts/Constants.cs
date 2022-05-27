using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public static int tetriminoHeight = 4;
    public static int tetriminoWidth = 4;
    public static int boardWidth = 10;
    public static int boardHeight = 20;
    public static int boardExtraHeight =3;

    //in order to make sure matrices are accessed via col, row, aka x,y. each entry in this matrix, is a column.
    public static int [,] initialGameBoard = {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};


      public static int [,] jMatrix ={{0,1,1,0},
                                      {0,2,0,0},
                                      {0,1,0,0},
                                      {0,0,0,0}};


      public static int [,] lMatrix ={{0,1,1,0},
                                      {0,0,2,0},
                                      {0,0,1,0},
                                      {0,0,0,0}};

      public static int [,] oMatrix ={{0,0,0,0},
                                      {0,1,1,0},
                                      {0,1,1,0},
                                      {0,0,0,0}};

      public static int [,] sMatrix ={{0,1,0,0},
                                      {0,2,1,0},
                                      {0,0,1,0},
                                      {0,0,0,0}};

      public static int [,] iMatrix ={{0,1,0,0},
                                      {0,1,0,0},
                                      {0,1,0,0},
                                      {0,1,0,0}};

      public static int [,] zMatrix ={{0,0,1,0},
                                      {0,2,1,0},
                                      {0,1,0,0},
                                      {0,0,0,0}};

      public static int [,] tMatrix ={{0,0,1,0},
                                      {0,1,2,0},
                                      {0,0,1,0},
                                      {0,0,0,0}};

      public static int [][ , ] tetriminoMatrices = {iMatrix, jMatrix, lMatrix, oMatrix, sMatrix, tMatrix, zMatrix};

      public static float lockTime = 1f;

      public static string[] tetriminoPrefabs = {"Assets/Prefabs/IPiece.prefab",
                                                 "Assets/Prefabs/JPiece.prefab",
                                                 "Assets/Prefabs/LPiece.prefab",
                                                 "Assets/Prefabs/OPiece.prefab",
                                                 "Assets/Prefabs/SPiece.prefab",
                                                 "Assets/Prefabs/TPiece.prefab",
                                                 "Assets/Prefabs/ZPiece.prefab"};


public static string[] tilePrefabs = {"Assets/Prefabs/I.square.prefab",
                                      "Assets/Prefabs/J.square.prefab",
                                      "Assets/Prefabs/L.square.prefab",
                                      "Assets/Prefabs/O.square.prefab",
                                      "Assets/Prefabs/S.square.prefab",
                                      "Assets/Prefabs/T.square.prefab",
                                      "Assets/Prefabs/Z.square.prefab"
                                      };
}

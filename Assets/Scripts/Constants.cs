using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Constants
{
    public static int tetriminoHeight = 4;
    public static int tetriminoWidth = 4;
    public static int boardWidth = 10;
    public static int boardHeight = 20;
    public static int boardExtraHeight =5;
    public static Color shadowColor = new Color();

    //in order to make sure matrices are accessed via col, row, aka x,y. each entry in this matrix, is a column.
    public static int [,] initialGameBoard = {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                              {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};


      public static int [,] jMatrix ={{0,1,1,0},
                                      {0,2,0,0},
                                      {0,1,0,0},
                                      {0,0,0,0}};


      public static int [,] lMatrix ={{0,0,0,0},
                                      {0,1,0,0},
                                      {0,2,0,0},
                                      {0,1,1,0}};

      public static int [,] oMatrix ={{0,0,0,0},
                                      {0,1,1,0},
                                      {0,1,1,0},
                                      {0,0,0,0}};

      public static int [,] sMatrix ={{0,1,0,0},
                                      {0,2,1,0},
                                      {0,0,1,0},
                                      {0,0,0,0}};

      public static int [,] iMatrix ={{0,0,1,0},
                                      {0,0,1,0},
                                      {0,0,1,0},
                                      {0,0,1,0}};

      public static int [,] zMatrix ={{0,0,1,0},
                                      {0,2,1,0},
                                      {0,1,0,0},
                                      {0,0,0,0}};

      public static int [,] tMatrix ={{0,0,0,0},
                                      {0,1,0,0},
                                      {0,2,1,0},
                                      {0,1,0,0}};

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

public static string[] tetriminoSprites = {"Assets/Art/Tetriminos/I.png",
                                           "Assets/Art/Tetriminos/J.png",
                                           "Assets/Art/Tetriminos/L.png",
                                           "Assets/Art/Tetriminos/O.png",
                                           "Assets/Art/Tetriminos/S.png",
                                           "Assets/Art/Tetriminos/T.png",
                                           "Assets/Art/Tetriminos/Z.png"};

public static float [,] tetriminoSignCoordinates = {{0.07f,-4f},
                                                      {0f, -4f},
                                                      {0.07f, -3.15f},
                                                      {0f,-3.5f},
                                                      {0f,-4f},
                                                      {0f,-3.15f},
                                                      {0f,-4f}
                                                    };

//enumerated list of all the possible positions a piece can take on the board ( 0-10 is x position, U L D R represent the orientation of the piece)
public enum Actions : int
{
  U0, U1, U2, U3, U4, U5, U6, U7, U8, U9,
  D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,
  L0, L1, L2, L3, L4, L5, L6, L7, L8, L9,
  R0, R1, R2, R3, R4, R5, R6, R7, R8, R9
}
public List<Actions>  Iactions =  new List<Actions> {Actions.U0, Actions.U1, Actions.U2,Actions.U3, Actions.U4, Actions.U5, Actions.U6, Actions.U7, Actions.U8, Actions.U9,
                                          Actions.L1, Actions.L2,Actions.L3, Actions.L4, Actions.L5, Actions.L6, Actions.L7};

public List<Actions>   JLTactions  =  new List<Actions>     {Actions.U1, Actions.U2,Actions.U3, Actions.U4, Actions.U5, Actions.U6, Actions.U7, Actions.U8,
                                          Actions.D1, Actions.D2,Actions.D3, Actions.D4, Actions.D5, Actions.D6, Actions.D7, Actions.D8,
                                          Actions.L1, Actions.L2,Actions.L3, Actions.L4, Actions.L5, Actions.L6, Actions.L7, Actions.L8,Actions.L9,
                                          Actions.R0, Actions.R1, Actions.R2,Actions.R3, Actions.R4, Actions.R5, Actions.R6, Actions.R7, Actions.R8};

public List<Actions>  Oactions  =  new List<Actions>  {Actions.U1, Actions.U2,Actions.U3, Actions.U4, Actions.U5, Actions.U6, Actions.U7, Actions.U8,Actions.U9};


public List<Actions>  SZactions  =  new List<Actions> {Actions.U1, Actions.U2,Actions.U3, Actions.U4, Actions.U5, Actions.U6, Actions.U7, Actions.U8,
                                          Actions.L1, Actions.L2,Actions.L3, Actions.L4, Actions.L5, Actions.L6, Actions.L7, Actions.L8,Actions.L9};



}

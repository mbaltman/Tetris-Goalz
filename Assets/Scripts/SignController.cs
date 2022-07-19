using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using static Constants;

public class SignController : MonoBehaviour
{
    public string pieceName;
    public GameObject piece;
    public GameObject menu;
    public Sprite [] tetriminoSprites;

    SpriteRenderer pieceRenderer;


    void Awake()
    {
      pieceRenderer = piece.GetComponent<SpriteRenderer>();
      menu.GetComponent<GameFlowManager>().UpdatePiece += UpdatePiece;
      menu.GetComponent<GameFlowManager>().VoidSign += Reset;
    }

    void UpdatePiece(int index, string currPiece)
    {
      if(currPiece == pieceName)
      {
        if( index == -1)
          pieceRenderer.sprite  = null;
        else
         {
            pieceRenderer.sprite = tetriminoSprites[index];
            piece.transform.localPosition = new Vector3(Constants.tetriminoSignCoordinates[index, 0], Constants.tetriminoSignCoordinates[index, 1], 0f);
            GetComponent<Animator>().Play("SignSwinging");
         }

      }

    }
    public void Reset()
    {
      pieceRenderer.sprite = null;
    }
}

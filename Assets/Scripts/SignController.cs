using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using static Constants;

public class SignController : MonoBehaviour
{
    public string pieceName;
    GameObject piece;
    SpriteRenderer pieceRenderer;
    public Sprite [] tetriminoSprites;

    void Awake()
    {
      piece =  GameObject.Find(pieceName);
      pieceRenderer = piece.GetComponent<SpriteRenderer>();
      GameObject.Find("Menu").GetComponent<GameFlowManager>().UpdatePiece += UpdatePiece;
      GameObject.Find("Menu").GetComponent<GameFlowManager>().VoidSign += Reset;
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

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
      GameObject.Find("PlayerObject").GetComponent<GameFlowManager>().UpdatePiece += UpdatePiece;
    }

    void UpdatePiece(int index, string currPiece)
    {
      if(currPiece == pieceName)
      {
        pieceRenderer.sprite = tetriminoSprites[index];
        piece.transform.localPosition = new Vector3(Constants.tetriminoSignCoordinates[index, 0], Constants.tetriminoSignCoordinates[index, 1], 0f);
        GetComponent<Animator>().Play("SignSwinging");
      }

    }
}

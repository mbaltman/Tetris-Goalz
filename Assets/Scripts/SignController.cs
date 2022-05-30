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
        pieceRenderer.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(Constants.tetriminoSprites[index], typeof(Sprite));
        piece.transform.localPosition = new Vector3(Constants.tetriminoSignCoordinates[index, 0], Constants.tetriminoSignCoordinates[index, 1], 0f);
        
      }
    }
}

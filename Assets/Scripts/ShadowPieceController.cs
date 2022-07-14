using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPieceController : MonoBehaviour
{
    private GameObject parent;
    private ActivePieceController parentController;

    private Matrix pieceMatrix;
    private GameBoardManager gameBoardManager;
    private SpriteRenderer pieceRenderer;

    void Awake()
    {
      parent = this.transform.parent.gameObject;
      parentController = parent.GetComponent<ActivePieceController>();
      gameBoardManager = GameObject.Find("Grid").GetComponent<GameBoardManager>();
      pieceRenderer = gameObject.GetComponent<SpriteRenderer>();
      pieceRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parent.transform.position;

        pieceMatrix = parentController.pieceMatrix;

        while(gameBoardManager.CheckMoveDown(transform.position, pieceMatrix))
        {
          transform.position = transform.position + new Vector3(0f,-1f,0f);
        }
        pieceRenderer.enabled = true;


    }
}

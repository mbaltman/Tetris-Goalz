using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameBoardManager;

public class TetriminoController : MonoBehaviour
{
    GameBoardManager gameBoardManager;
    bool deleteMyself;
    bool moveDown;
    float distanceDown;
    private float timeToMove;

    // Start is called before the first frame update
    void Awake()
    {
     GameObject.Find("Grid").GetComponent<GameBoardManager>().ClearLine += CheckMyself;
     deleteMyself = false;
     moveDown = false;
     distanceDown = 0f;


    }

    // Update is called once per frame
    void Update()
    {
      if(deleteMyself)
      {
        Debug.Log("KILLL MEEEE");
        GameObject.Find("Grid").GetComponent<GameBoardManager>().ClearLine -= CheckMyself;
        GameObject.Destroy(gameObject);
      }
      else if(moveDown && Time.time > timeToMove)
      {
        transform.position = transform.position + new Vector3(0f,-distanceDown,0f);
        distanceDown= 0f;
        moveDown = false;
      }

    }

    //check yourself before you wreck yourself
    public void CheckMyself(int row)
    {
      if( (int) transform.position.y == row)
      {
        deleteMyself = true;

      }
      else if((int) transform.position.y > row)
      {
        moveDown = true;
        distanceDown += 1f;
        // add in delay, for clear line animation
        timeToMove = Time.time + Constants.lineClearTime;

      }
    }

}

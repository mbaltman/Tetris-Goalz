using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameBoardManager;

public class TetriminoController : MonoBehaviour
{
    GameBoardManager gameBoardManager;

    // Start is called before the first frame update
    void Awake()
    {
     GameObject.Find("Grid").GetComponent<GameBoardManager>().ClearLine += CheckMyself;


    }

    // Update is called once per frame
    void Update()
    {

    }

    //check yourself before you wreck yourself
    public void CheckMyself(int row)
    {
      if( (int) transform.position.y == row)
      {
        Debug.Log("KILLL MEEEE");
        GameObject.Find("Grid").GetComponent<GameBoardManager>().ClearLine -= CheckMyself;
        GameObject.Destroy(gameObject);
      }
      else if((int) transform.position.y > row)
      {
        transform.position = transform.position + new Vector3(0f,-1f,0f);
      }
    }

}

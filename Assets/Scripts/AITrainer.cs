using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
public class AITrainer
{
  //                height          top four row  current piece: value
  public Dictionary<int , Dictionary<long, Dictionary< byte, float>>> qtable;
  public float epsilon; //exploration rate
  public float alpha; //learning rate
  public float gamma; //future reward balance
    // Start is called before the first frame update
    public AITrainer ()
    {
      qtable = new Dictionary<int , Dictionary<long, Dictionary< byte, float>>> {};
    }


    public void Step(int pieceIndex)
    {


    }

    private int getMove( int pieceIndex)
    {
      return -1;
    }
}

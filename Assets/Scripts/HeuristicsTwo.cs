using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicsTwo
{
    public int[] pos = new int[2];
    public HeuristicsTwo lastPos;

   public HeuristicsTwo(int PosX, int PosY,HeuristicsTwo lastPos)
    {
        pos[0] = PosX;
        pos[1] = PosY;
        this.lastPos = lastPos;
    }

    public bool Equals(HeuristicsTwo e)
    {
        if(e.pos == this.pos)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}

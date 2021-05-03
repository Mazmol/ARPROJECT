using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristics : MonoBehaviour
{
    public int g = 0;
    public int h = 0;
    public int f = 0;
    public int x;
    public int y;
    public GameObject papa;

  

    

    public void setG(int g)
    {
        this.g = g;
        this.f = this.g + this.h;
    }

    public void setH(int h)
    {
        this.h = h;
        this.f = this.g + this.h;
    }

    public void setGH(int g, int h)
    {
        this.h = h;
        this.g = g;
        this.f = this.g + this.h;
    }

    public void setPos(int x,int y)
    {
        this.x = x;
        this.y = y;
    }

    public int[] getPos()
    {
        int[] pos = new int[2] { x, y };
        return pos;
    }

    public bool EQ(GameObject other)
    {
       
        return ((x == other.GetComponent<Heuristics>().x) && (y == other.GetComponent<Heuristics>().y));
    }
   

}

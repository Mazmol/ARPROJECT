using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridMaker : MonoBehaviour
{
    //Prefabs
    public GameObject plane;
    public GameObject wall;
    public GameObject borderwall;
    public GameObject lStart;
    public GameObject lEnd;
    public int[] pathfindingEssentials; //0,1 is x,y of start. 2,3 is end.
 
    

    //Grid Size
    public int width;
    public int height;
    public int offset; //distance to middle of the grid square.
    public int spacing; //distance between squares in units.

    private int[] level;

    public float scale = 1; //1 is 1x, 2 is 2x, ect.

    public GameObject[,] gridAxis;

    // Start is called before the first frame update
    void Start()
    {
        gridAxis = new GameObject[width, height];

        //0 floor.
        //1 Wall.
        //2 + 3 is Start + End
        level = new int[] 
       {0,0,1,2,1,0,0,0,0,0,
        0,0,1,0,1,0,0,0,0,0,
        0,0,1,0,1,1,1,1,1,0,
        0,0,1,0,0,0,0,0,1,0,
        0,0,1,1,1,1,1,0,1,0,
        0,0,0,0,0,0,1,0,1,0,
        0,0,0,0,0,0,1,0,1,0,
        0,0,0,0,0,0,1,0,1,0,
        0,0,0,0,1,1,1,0,1,0,
        0,0,0,0,1,3,0,0,1,0};

        pathfindingEssentials = new int[4];

        makeLevel();
        transform.localScale = new Vector3(scale,scale,scale);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void testmode()
    {
        for (int i = 0; i < width; i++)
        {
            for (int n = 0; n < height; n++)
            {
                float isWall = Random.Range(1, 10);
                if (isWall % 2 == 0)
                {
                    gridAxis[i, n] = Instantiate(plane, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i + 1) + " | " + (n + 1);
                    //Debug.Log(gridAxis);
                }

                else
                {
                    gridAxis[i, n] = Instantiate(wall, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i+1) + " | " + (n+1);

                    //Debug.Log(gridAxis);
                }


            }
        }

        for (int i = 0; i < width + 2; i++)
        {
            Instantiate(borderwall, new Vector3((i * spacing) - offset, 0, 0 - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3(0 - offset, 0, (i * spacing) - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3((height * spacing) + offset, 0, (i * spacing) - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3((i * spacing) - offset, 0, (height * spacing) + offset), Quaternion.identity, transform);
        }
    }

    void makeLevel(){
        if(level.Length != width*height)
        {
            level = new int [width*height];
            for (int i = 0; i < level.Length; i++)
            {
                level[i] = 0;
            }
        }

        int pointer = 0;
        for (int i = 0; i < width; i++)
        {
            for (int n = 0; n < height; n++)
            {
               
                if (level[pointer] == 0)
                {
                    gridAxis[i, n] = Instantiate(plane, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i + 1) + " | " + (n + 1);
                    //Debug.Log(gridAxis);
                    gridAxis[i, n].GetComponent<Heuristics>().setPos(i,n);
                    pointer++;
                }

                else if(level[pointer] == 2)
                {
                    gridAxis[i, n] = Instantiate(lStart, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i + 1) + " | " + (n + 1);
                    //Debug.Log(gridAxis);
                    gridAxis[i, n].GetComponent<Heuristics>().setPos(i, n);
                    pathfindingEssentials[0] = i;
                    pathfindingEssentials[1] = n;
                    pointer++;
                }

                else if(level[pointer] == 3)
                {
                    gridAxis[i, n] = Instantiate(lEnd, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i + 1) + " | " + (n + 1);
                    gridAxis[i, n].GetComponent<Heuristics>().setPos(i, n);
                    pathfindingEssentials[2] = i;
                    pathfindingEssentials[3] = n;
                    //Debug.Log(gridAxis);
                    pointer++;
                }

                else
                {
                    gridAxis[i, n] = Instantiate(wall, new Vector3((i * spacing) + offset, 0, (n * spacing) + offset), Quaternion.identity, transform) as GameObject;
                    gridAxis[i, n].name = (i + 1) + " | " + (n + 1);

                    //Debug.Log(gridAxis);
                    gridAxis[i, n].GetComponent<Heuristics>().setPos(i, n);
                    pointer++;
                }


            }
        }

        for (int i = 0; i < width + 2; i++)
        {
            Instantiate(borderwall, new Vector3((i * spacing) - offset, 0, 0 - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3(0 - offset, 0, (i * spacing) - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3((height * spacing) + offset, 0, (i * spacing) - offset), Quaternion.identity, transform);
            Instantiate(borderwall, new Vector3((i * spacing) - offset, 0, (height * spacing) + offset), Quaternion.identity, transform);
        }
    }

 
}

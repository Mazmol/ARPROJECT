using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GameObject[,] gridMatrix;
    public GameObject fool;
    public List<GameObject> path;
    public int[] startEnd;
    public int[,] dirs;
    public int[] size;

    // Start is called before the first frame update
    void Start()
    {
        dirs = new int[,] { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };
        size = new int[2];
}

    // Update is called once per frame
    void Update()
    {
        if (gridMatrix == null)
        {
            gridMatrix = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().gridAxis;
            startEnd = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().pathfindingEssentials;
            Debug.Log(startEnd[0] + "," + startEnd[1]);
            Debug.Log(startEnd[2] + "," + startEnd[3]);
            size[0] = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().width;
            size[1] = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().height;

            path = AStar(gridMatrix,startEnd[0],startEnd[1],startEnd[2],startEnd[3]);
            Debug.Log(path);
            for(int i = 0; i < path.Count; i++)
            {
                Instantiate(fool, path[i].transform.position, Quaternion.identity, transform);
            }

        }
    }

    public List<GameObject> AStar(GameObject[,] Level, int startX, int startY, int endX, int endY)
    {
        //Declare the Start Points as Tiles
        GameObject start_point = Level[startX, startY];
        start_point.GetComponent<Heuristics>().papa = null;
        start_point.GetComponent<Heuristics>().setG(0);
        start_point.GetComponent<Heuristics>().setH(0);

        GameObject end_point = Level[endX, endY];
        end_point.GetComponent<Heuristics>().setG(0);
        end_point.GetComponent<Heuristics>().setH(0);

        //Declare and Open List and Closed List. Closed list is "This is not that tile" Open List is every tile we search
        List<GameObject> openList = new List<GameObject>();
        List<GameObject> closedList = new List<GameObject>();

        //add the opening node to the start list
        openList.Add(start_point);

        while (openList.Count > 0)
        {
            //current node is the first until sort.
            GameObject current_node = openList[0];
            bool alreadySeen = false;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].GetComponent<Heuristics>().f < current_node.GetComponent<Heuristics>().f)
                {
                    current_node = openList[i];
                }
            }
            Debug.Log(current_node == null);
            //take the lowest F and put it on the closed list.
            openList.Remove(current_node);
            closedList.Add(current_node);

            //while we have it in memory, hey this isn't the right square yes?
            //Debug.Log(current_node.GetComponent<Heuristics>().EQ(end_point));
            if (current_node.GetComponent<Heuristics>().EQ(end_point))
            {
                //found
                Debug.Log(current_node.GetComponent<Heuristics>().x + "," + current_node.GetComponent<Heuristics>().y);
                List<GameObject> path = new List<GameObject>();
                GameObject current = current_node;
                int index = 0;

                while (current != null)
                {
                    //Debug.Log(index);
                    path.Add(current);
                    Debug.Log(current.GetComponent<Heuristics>().x + "," + current.GetComponent<Heuristics>().y);
                    current = current.GetComponentInChildren<Heuristics>().papa;



                    index++;
                    if (index>5)
                    { current = null; }
                }
                return path;
            }

            //Let's check in with the neighbors. Make an Array of All Children
            List<GameObject> children = new List<GameObject>();

            //We're 4 directional so we only check x +/-1 and y +/-1 seperately. (-1,0 0,-1 1,0 0,1)
            for (int i = 0; i < 4; i++)
            {
                if (current_node.GetComponent<Heuristics>().x + dirs[i, 0] >= 0 &&
                    current_node.GetComponent<Heuristics>().y + dirs[i, 1] >= 0 &&
                    current_node.GetComponent<Heuristics>().x + dirs[i, 0] < size[0] &&
                    current_node.GetComponent<Heuristics>().y + dirs[i, 1] < size[1])
                {
                    //Debug.Log("Origin");
                    //Debug.Log(current_node.GetComponent<Heuristics>().x + "," + current_node.GetComponent<Heuristics>().y);
                    //Debug.Log("X");
                    //Debug.Log(current_node.GetComponent<Heuristics>().x + dirs[i, 0]);
                    //Debug.Log("Y");
                    //Debug.Log(current_node.GetComponent<Heuristics>().y + dirs[i, 1]);
                    GameObject newpos = Level[(current_node.GetComponent<Heuristics>().x) + dirs[i, 0], (current_node.GetComponent<Heuristics>().y + dirs[i, 1])];

                    if (newpos.tag != "Wall")
                    {
                       // Debug.Log(newpos.tag);
                        newpos.GetComponent<Heuristics>().papa = current_node;
                        children.Add(newpos);

                    }

                    
                }
            }

            Debug.Log("Length: " + children.Count);

            //we've made a list of neighbors. Looping!
            for (int i = 0; i < children.Count; i++)
            {
                alreadySeen = false;
                //Debug.Log(i);
                //save some time, let's just check we're not backtracking
                for(int n = 0; n < closedList.Count; n++)
                {
                    if (children[i].GetComponent<Heuristics>().EQ(closedList[i]))
                    {

                        alreadySeen = true;
                        Debug.Log("Seen closed");
                        
                    }
                }

                if (alreadySeen)
                {
                    continue;
                }
                Debug.Log("Closed Check Passed");

                //F = G+H. F is overall cost. G is the distance from the start, thus being the current node +1. H is distance from this tile to the end.
                children[i].GetComponent<Heuristics>().setG(current_node.GetComponent<Heuristics>().g + 1);
                
                children[i].GetComponent<Heuristics>().setH(distanceTo(children[i],end_point));

                //Debug.Log(children[i].GetComponent<Heuristics>().g + " " + children[i].GetComponent<Heuristics>().h);

                //And again, this isn't on the list already AND it isn't farther from the start than this tile?
                for(int n =0; n<openList.Count; n++)
                {
                    if (children[i].GetComponent<Heuristics>().EQ(openList[n]) && children[i].GetComponent<Heuristics>().g > openList[n].GetComponent<Heuristics>().g)
                    {
                        alreadySeen = true;
                    }

                    

                    
                }

                if (alreadySeen == true)
                {
                    Debug.Log("Continueing");   
                    continue;
                }

                openList.Add(children[i]);
                Debug.Log("Added Child");
                Debug.Log(children.Count);
            }



            children.Clear();
            
        }
        Debug.Log("Nope!");
        return null;
    }

    public int pythagoras(float a, float b)
    {
        //wait unity has no pythagoras? sucks.
        //a^2 + b^2 = c^2
        
        float c = Mathf.Sqrt((a * a) + (b * b));
        return (int) c;
    }

    private int distanceTo(GameObject a, GameObject b)
    {
        float x = a.GetComponent<Heuristics>().x - b.GetComponent<Heuristics>().x;
        float y = a.GetComponent<Heuristics>().y - b.GetComponent<Heuristics>().y;

        return pythagoras(x, y);
    }
}

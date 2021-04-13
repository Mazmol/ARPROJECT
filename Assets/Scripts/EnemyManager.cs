using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject[,] gridMatrix;
    private List<GameObject> levelPath;
    public int[] startEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gridMatrix == null)
        {
            gridMatrix = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().gridAxis;
            startEnd = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().pathfindingEssentials;
            Debug.Log(gridMatrix);
            levelPath = AStar(gridMatrix, new int[]{ startEnd[0], startEnd[1]}, new int[] { startEnd[2], startEnd[3]});
        }


    }

    private List<GameObject> FinalPath()
    {
        return null;
    }

    private List<GameObject> AStar(GameObject[,] Maze,int[] Start, int[] End)
    {
        int[] dir = { -1, 1};
        GameObject start_point = Maze[Start[0], Start[1]];
        start_point.GetComponent<Heuristics>().papa = null;


        GameObject end_point = Maze[End[0], End[1]];

        List<GameObject> openlist = new List<GameObject>();
        List<GameObject> closedList= new List<GameObject>();
     

        openlist.Add( start_point );

        while(openlist.Count > 0)
        {
           GameObject currPos = openlist[0];
            int currIndex = 0;
            for(int i=0; i < openlist.Count; i++)
            {
                if (openlist[i].GetComponent<Heuristics>().f < currPos.GetComponent<Heuristics>().f)
                {
                    currPos = openlist[i];
                    currIndex = i;
                }
            }

            openlist.Remove(currPos);
            closedList.Add(currPos);

            if(currPos.Equals(Maze[End[0], End[1]]))
            {
                List<GameObject> address = new List<GameObject>();
                GameObject current = currPos;
                while(current != null)
                {
                    address.Add(current);
                    current = current.GetComponent<Heuristics>().papa;
                }
                return address;
            }


            //Let's find everywhere else
            List<GameObject> children = new List<GameObject>();

            // For Each Neighbor
            Debug.Log(currPos.GetComponent<Heuristics>().x);
            for (int i = 0; i < dir.Length; i++)
            {
                Debug.Log(i);
                Debug.Log(dir[i]);
                //x+1,y/x-1 + y

                GameObject newPos;
                Debug.Log(currPos.GetComponent<Heuristics>().x + dir[i] >= 0 && currPos.GetComponent<Heuristics>().y + dir[i] >= 0);
                if (currPos.GetComponent<Heuristics>().x + dir[i] >= 0 && currPos.GetComponent<Heuristics>().y + dir[i] >= 0)
                {
                    newPos = Maze[(currPos.GetComponent<Heuristics>().x) + dir[i], (currPos.GetComponent<Heuristics>().y)];

                    if (newPos.tag != "Wall")
                    {
                        newPos.GetComponent<Heuristics>().papa = currPos;
                        children.Add(newPos);
                    }

                    //x,y+1/x,y-1
                    newPos = Maze[(currPos.GetComponent<Heuristics>().x), (currPos.GetComponent<Heuristics>().y) + dir[i]];
                    if (newPos.tag != "Wall")
                    {
                        newPos.GetComponent<Heuristics>().papa = currPos;
                        children.Add(newPos);
                    }
                }
                
            }

            //Figure out Children's role in all this.
            foreach(GameObject child in children)
            {
                child.GetComponent<Heuristics>().setG(currPos.GetComponent<Heuristics>().g);
                child.GetComponent<Heuristics>().setH( ((currPos.GetComponent<Heuristics>().x-end_point.GetComponent<Heuristics>().x)*
                                                        (currPos.GetComponent<Heuristics>().x - end_point.GetComponent<Heuristics>().x) ) +
                                                        (currPos.GetComponent<Heuristics>().y - end_point.GetComponent<Heuristics>().y)*
                                                        (currPos.GetComponent<Heuristics>().y - end_point.GetComponent<Heuristics>().y));

                foreach(GameObject openchild in openlist)
                {
                    if (child == openchild && child.GetComponent<Heuristics>().g > child.GetComponent<Heuristics>().g )
                    {
                        openlist.Add(child);
                    }
                }

                
            }



        }
        Debug.Log("Fuck!");
        return null;
        
    }

    
}


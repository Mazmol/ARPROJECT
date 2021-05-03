using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyPathfinder : MonoBehaviour
{

    /// <summary>
    /// PathFinding Variables
    /// </summary>
    public GameObject[,] gridMatrix;
    public List<GameObject> path;
    public int[] startEnd;
    public int[] size; //the size of the grid. Important for processing.
    public int maxLoops = 10000;
    List<HeuristicsTwo> OpenList;
    List<HeuristicsTwo> ClosedList;


    //Enemy Variables
    public GameObject Enemy;
    public GameObject[] EnemyTypes; //0 = normal. 1 = Speedy. 2 = STRONK. 3 is the alien if it works.;
    public int[] WaveForecast; //Each int is a wave with how many enemies.
    public int currWave = 0;
    public List<GameObject> EnemyList;

    private int EnemiesSpawned;
    private bool canSpawn = true;
    public float spawnInterval;
    public float timer;
    private bool levelWin = false;


    // Start is called before the first frame update
    void Start()
    {
        size = new int[2];
         OpenList = new List<HeuristicsTwo>();
        ClosedList = new List<HeuristicsTwo>();
        EnemiesSpawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gridMatrix == null)
        {
            gridMatrix = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().gridAxis;
            startEnd = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().pathfindingEssentials;
            
            size[0] = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().width;
            size[1] = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().height;

            path = Pathfind(gridMatrix, startEnd[0], startEnd[1], startEnd[2], startEnd[3]);
            Debug.Log(path);
            //for (int i = 0; i < path.Count; i++)
            //{
                //Instantiate(Enemy, path[i].transform.position, Quaternion.identity, transform);
            //}

        }

        else
        {
            timer += Time.deltaTime;
            
            if (EnemiesSpawned < WaveForecast[currWave] && canSpawn)
            {
                GameObject enemy;

                if (EnemiesSpawned > (WaveForecast[currWave] / 2) && EnemiesSpawned+1 != WaveForecast[currWave])
                {
                    enemy = Instantiate(EnemyTypes[1], path[0].transform.position, Quaternion.identity, transform);
                }

                else if (EnemiesSpawned+1 == WaveForecast[currWave])
                {
                     enemy = Instantiate(EnemyTypes[2], path[0].transform.position, Quaternion.identity, transform);
                }

                else
                {
                    enemy = Instantiate(EnemyTypes[0], path[0].transform.position, Quaternion.identity, transform);
                }
                
                EnemyList.Add(enemy);
                EnemiesSpawned++;
                canSpawn = false;
            }
            
            for(int i=0;i<EnemyList.Count;i++)
            {
                GameObject Enemy = EnemyList[i];
                if (Enemy.GetComponent<EnemyAI>().HP < 0)
                {

                    EnemyList.RemoveAt(i);
                    Destroy(Enemy);
                }
            }

            for (int i=0;i<EnemyList.Count;i++)
            {
                GameObject Enemy = EnemyList[i];
                EnemyAI Ai = EnemyList[i].GetComponent<EnemyAI>();
                if(Ai.destinationFound && Ai.Index != path.Count-1)
                {
                    Ai.Index++;
                }
                

                if(Ai.destinationFound && Ai.Index == path.Count-1)
                {
                    Ai.HP = -1;
                    GetComponent<HP_Manger>().takeDamage(1);
                }

                Ai.SetTarget(path[Ai.Index].transform.position);

            }

            if (timer > spawnInterval && canSpawn == false)
            {
                timer = 0;
                canSpawn = true;
            }

            if(EnemiesSpawned == WaveForecast[currWave] && EnemyList.Count == 0)
            {
                if(currWave == WaveForecast.Length-1)
                {
                    levelWin = true;
                }

                else if(currWave < WaveForecast.Length-1)
                {
                    EnemiesSpawned = 0;
                    currWave++;
                }

            }

        }
    }

    public List<GameObject> Pathfind(GameObject[,] Level, int startX, int startY, int endX, int endY)
    {
        

        HeuristicsTwo endPos = new HeuristicsTwo(endX, endY, null);
        HeuristicsTwo endPosCopy = new HeuristicsTwo(endX, endY, endPos);



        OpenList.Add(new HeuristicsTwo(startX, startY,null));
        

        int iterations = 0;

        Debug.Log("Open List:" + " " + OpenList.Count);

        while (OpenList.Count > 0)
        {
            HeuristicsTwo currCell = OpenList[0];
            OpenList.Remove(currCell);
           

            if(NavCheck(currCell,endPos))
            {
                iterations = 0;
                List<GameObject> Path = new List<GameObject>();
                
                while(currCell != null)
                {
                    GameObject PathCell = Level[currCell.pos[0], currCell.pos[1]];
                    Path.Add(PathCell);
                    currCell = currCell.lastPos;
                    iterations++;
                    if(iterations>maxLoops)
                    {
                        Debug.Log("Max Loops Exceeded at the end");
                        return null;
                    }
                }
                Path.Reverse();
                return Path;
                

            }

            

            iterations++;
            if(iterations>maxLoops)
            {
                Debug.Log("Max Loops Exceeded normally");
                return null;
            }

            
        }

        Debug.Log("Loop Exited");
        return null;
    }

    public bool CanMoveHere(int x, int y)
    {
        Debug.Log(size[0] + "," + size[1]);
        if(x<0 || y<0 || x>size[0]-1 || y>size[1]-1)
        {
            return false;
        }
        Debug.Log(x);
        Debug.Log(y);

        return gridMatrix[x,y].tag != "Wall";
    }

    public bool NavCheck(HeuristicsTwo currPos, HeuristicsTwo end)
    {
        if(!CanMoveHere(currPos.pos[0],currPos.pos[1]))
        {
            return false;
        }

        if(isInList(currPos,ClosedList))
        {
            return false;
        }

        ClosedList.Add(currPos);

        if(currPos.pos[0] == end.pos[0] && currPos.pos[1] == end.pos[1])
        {
            return true;
        }

            OpenList.Add(new HeuristicsTwo(currPos.pos[0], currPos.pos[1] + 1, currPos));
            OpenList.Add(new HeuristicsTwo(currPos.pos[0] + 1, currPos.pos[1], currPos));
            OpenList.Add(new HeuristicsTwo(currPos.pos[0], currPos.pos[1] - 1, currPos));
            OpenList.Add(new HeuristicsTwo(currPos.pos[0] - 1, currPos.pos[1], currPos));


        return false;
    }

    public bool isInList(HeuristicsTwo e,List<HeuristicsTwo> list)
    {
        for(int i =0; i<list.Count;i++)
        {
            if(list[i].pos[0] == e.pos[0] && list[i].pos[1] == e.pos[1])
            {
                return true;
            }
        }

        return false;
    }

    
}

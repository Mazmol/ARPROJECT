using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentManger : MonoBehaviour 
{
    public LayerMask clickMask;
    public GameObject[,] gridMatrix;
    public GameObject prefab;
    public int cost;
    public int cap;
    private int built;
    private CashManager bank;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update(){
            if(Input.GetMouseButtonDown(0)) {
            Vector3 clickPostion = -Vector3.one;
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if (Physics.Raycast(ray, out hit,500f,clickMask)) {
               clickPostion = hit.point;
               {
                   if (Input.GetMouseButtonDown(0) && built <= cap && bank.GetCoins() >= cost)
                   {
                       Instantiate(prefab,hit.point, transform.rotation); // Comment add a fixed hight and prefab
                        bank.SpendCoin(cost);
                       built++;

                   }
               } 
            }
            

            // Debug Log 
            Debug.Log(clickPostion);

        }

            if(gridMatrix == null)
        {
            gridMatrix = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridMaker>().gridAxis;
        }
    }
}
             
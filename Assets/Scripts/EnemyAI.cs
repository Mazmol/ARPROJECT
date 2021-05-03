using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Vector3 TargetLocation; //Find the Next Bit.
    public int Index = 1;//Variable for the Manager.
    public bool destinationFound;
    public float speed = 1;

    public float HP = 10;
    public bool flying = false;
    public bool alive = true;

  

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!destinationFound )
        {
            transform.LookAt(TargetLocation);
            float distance = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetLocation, distance);
            
            

        }

        if (transform.position == TargetLocation)
        {
            destinationFound = true;
        }
    }

    public void Attack()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HP = HP - damage;
    }

    public void SetTarget(Vector3 target)
    {
        TargetLocation = target;
        TargetLocation.y = 0.001f;
        destinationFound = false;
    }

    public Vector3 getTargetLocation()
    {
        return TargetLocation;
    }
}

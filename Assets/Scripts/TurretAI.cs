using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{

    SphereCollider coll;
    List<GameObject> PotentialTargets;
    public int damage = 2;
    public float FireRate = 5; //Fire Rate Measured in Seconds;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (PotentialTargets.Count>0 && (timer > FireRate) )
        {
            GameObject target = PotentialTargets[0];
            //Code that makes turret point at enemy;
            transform.rotation = Quaternion.LookRotation( target.transform.position);
            GetComponent<ParticleSystem>().Play();
            Fire(target);
            timer = 0;
        }

        SanityCheck();
    }

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        PotentialTargets = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "enemy")
        {
            Debug.Log("An Enemy!");
            PotentialTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "enemy")
        {
            Debug.Log("Are you still there?");
            PotentialTargets.Remove(other.gameObject);
        }
    }

    private void Fire(GameObject Enemy)
    {
        EnemyAI ai = Enemy.GetComponent<EnemyAI>();

        if( (ai.HP - damage) < 0)
        {
            PotentialTargets.Remove(Enemy);
            ai.TakeDamage(damage);
        }

        else
        {
            ai.TakeDamage(damage);
        }

        Debug.Log("BANG!");
    }

    private void SanityCheck()
    {
        for(int i = 0; i<PotentialTargets.Count; i++)
        {
            if(PotentialTargets[i] == null)
            {
                PotentialTargets.RemoveAt(i);
            }
        }
    }


}

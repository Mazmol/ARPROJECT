using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HP_Manger : MonoBehaviour
{
    public bool gamelost;
    public int hp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (gamelost){
            SceneManager.LoadScene(2);
           
         }       
    }

    public void takeDamage(int hp)
    {
        this.hp = this.hp - hp ;
        if (this.hp<=0){
             gamelost = true;
        }
    }
}

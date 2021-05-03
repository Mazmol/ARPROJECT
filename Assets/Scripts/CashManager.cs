using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    int Coins;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCoins()
    {
        return Coins;
    }

    public bool SpendCoin(int toSpend)
    {
        if(Coins - toSpend > -1)
        {
            Coins = Coins - toSpend;
            return true;
        }

        return false;
    }

    public void EarnCoin(int toEarn)
    {
        Coins = Coins + toEarn;
    }
}

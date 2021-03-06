﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SquadExp : NetworkBehaviour {

    public int money
    {
        get { return _money; }
    }

    [SyncVar]
    private int _money = 10;

    private float squadExpTotal = 0;
    private float squadExp = 0;
    private int expToMoney = 100; //100 exp = 1 money

    /*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    */

    public void GainExp(float amount)
    {
        squadExpTotal += amount;
        squadExp += amount;
        if ( squadExp > expToMoney)
        {
            conwertExtToMoney();
        }
    }

    public float GetTotalExp()
    {
        return squadExpTotal;
    }

    void conwertExtToMoney()
    {
        int newMoney = (int)squadExp / expToMoney;
        squadExp = squadExp % expToMoney;
        if ( newMoney > 0 )
        {
            addMoney(newMoney);
        }

    }

    void addMoney(int newMoney)
    {
        if (newMoney > 0)
        {
            _money += newMoney;
            reportAboutMoneyIncome();
        }
    }

    //todo
    void reportAboutMoneyIncome()
    {
        //todo add money here
    }

    public bool spendMoney(int amount)
    {
        bool res = false;

        if ( amount <= _money)
        {
            CmdSpendMoney(amount);
            
            res = true;
            reportAboutMoneyIncome();
        }

        return res;
    }

    [Command]
    void CmdSpendMoney(int amount)
    {
        _money -= amount;
    }

}
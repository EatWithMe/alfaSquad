using UnityEngine;
using System.Collections;

public class SquadExp : MonoBehaviour {

    public int money
    {
        get { return _money; }
    }

    private int _money = 0;
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

    public void getSquadExp(float amount)
    {
        squadExpTotal += amount;
        squadExp += amount;
        if ( squadExp > expToMoney)
        {
            conwertExtToMoney();
        }
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
            _money -= amount;
            res = true;
            reportAboutMoneyIncome();
        }

        return res;
    }

}
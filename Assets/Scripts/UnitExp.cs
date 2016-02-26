using UnityEngine;
using System.Collections;

public enum ExpAction
{ 
    Reload = 0, //reload is autimatic - this means that we cannot abuse it
    FreeLevelUp,
    Kill
}


public class UnitExp : MonoBehaviour {

    private int _level = 0;

    private float expCurrent = 0;

    private float expThisLevel = 0;
    private float expNextLevel = 100; // 1 1 2 3 5 8 12 20
    static int expBase = 100; // exp amount to gaint first level

    public int level
    {
        get { return _level; }
    }


    public void CommitAction(ExpAction act)
    {
        expCurrent += GetActionExpAmount(act);
        if (CheckForLevelUp()) LevelUpReport();
    }

    bool CheckForLevelUp()
    {
        bool res = false; 
         if ( expCurrent > expNextLevel)
        {
            _level++;
            float tmpL = expNextLevel;
            expNextLevel = expNextLevel + expThisLevel;
            expThisLevel = tmpL;
            res = true;
            CheckForLevelUp(); //we need to check big amount of exp - multiple lvlup
        }

        return res;
    }

    void LevelUpReport()
    {

    }


    float GetActionExpAmount(ExpAction act)
    {
        float res = 1;

        switch (act)
        {
            case ExpAction.Reload:
                res = 10;
                break;
            case ExpAction.FreeLevelUp:
                res = expNextLevel - expThisLevel ;
                break;
            case ExpAction.Kill:
                res = expBase;
                break;
            default:
                res = 0;
                //Debug.LogError("Unrecognized act");
                break;
        }


        return res;
    }

    public void GainExp(float amount)
    {
        expCurrent += amount;
        CheckForLevelUp();

        //report to squad about unitPesonalexp
        SendMessageUpwards("GainExp",amount);
    }
}

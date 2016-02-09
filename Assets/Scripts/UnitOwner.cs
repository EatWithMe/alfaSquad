using UnityEngine;
using System.Collections;

//[System.Serializable]
public class UnitOwner : MonoBehaviour {

    //public string playerName = "Player";

    [SerializeField]
    private string _playerName = "Player";
    /// <summary>
    /// -1 - all are enemyes
    /// 0 - N team index
    /// </summary>
    [SerializeField]
    private int _teamIndex = -1;
    [SerializeField]
    private int _playerIndex = -1;






    private static bool friendlyFire = false;


    private UnitOwner parent = null;

    public void setOwnerShip( UnitOwner owner)
    {

        if (owner != null)
        {
            parent = owner;
            this.playerName = owner.playerName;
            this.playerIndex = owner.playerIndex;
            this.teamIndex = owner.teamIndex;
        }
    }


    public string playerName
    {
        get
        {
            if (parent)
            {
                return parent.playerName;
            }
            else {
                return _playerName;
            }
        }
        set { _playerName = value; }
    }

    public int teamIndex
    {
        get
        {
            if (parent)
            {
                return parent.teamIndex;
            }
            else {
                return _teamIndex;
            }
        }
        set { _teamIndex = value; }

    }

    public int playerIndex
    {
        get
        {
            if (parent)
            {
                return parent.playerIndex;
            }
            else {
                return _playerIndex;
            }
        }
        set { _playerIndex = value; }

    }








    public static bool isFriendly(UnitOwner p1 , UnitOwner p2)
    {
        bool res = false;
        if ( p1.teamIndex >= 0)
        {
            if (p1.teamIndex == p2.teamIndex) res = true;
        }
        return res;
    }

    public static bool canHeShootHim(UnitOwner p1, UnitOwner p2)
    {
        bool res = true;

        if (friendlyFire == true)
        {
            res = true;
        }
        else
        {
            res = ! isFriendly(p1, p2); ;
        }
        return res;
    }

}


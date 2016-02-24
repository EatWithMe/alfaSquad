using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[System.Serializable]
public class UnitOwner : NetworkBehaviour {

    //public string playerName = "Player";

    //[SerializeField]
    [SyncVar]
    public string playerName = "Player";
    //private string _playerName = "Player";
    /// <summary>
    /// 0 - all are enemyes
    /// 1 - N team index
    /// </summary>
    /// 
    //[SerializeField]
    [SyncVar]
    public int teamIndex = 0;
    //private int _teamIndex = -1;

    //[SerializeField]
    //public int playerIndex = -1;
    //private int _playerIndex = -1;

    [SyncVar]
    public NetworkInstanceId playerNetId;



    private static bool friendlyFire = false;


    //private UnitOwner parent = null;

    public void setOwnerShip( UnitOwner owner)
    {

        if (owner != null)
        {
            //parent = owner;
            this.playerName = owner.playerName;
            //this.playerIndex = owner.playerIndex;
            this.playerNetId = owner.playerNetId;
            this.teamIndex = owner.teamIndex;
        }
    }

    /*
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
    */







    public static bool isFriendly(UnitOwner p1 , UnitOwner p2)
    {
        bool res = false;

        if (p1.playerNetId == p2.playerNetId)
        {
            res = true;
        }
        else
        {
            if (p1.teamIndex > 0)
            {
                if (p1.teamIndex == p2.teamIndex) res = true;
            }
            else
            {
                //res = false;
            }
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
            res = !  UnitOwner.isFriendly(p1, p2); ;
        }
        return res;
    }

}


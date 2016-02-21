using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MyNetworkManager : NetworkManager
{

/*
    private int playerCount = 0;
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        //Network.RemoveRPCs(player);
        //Network.DestroyPlayerObjects(player);
    }

    */

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("On COnnect");

        // The code in here runs fine when a Client connects to the Server.
        base.OnServerConnect(conn);
        //NetworkManager.singleton.

    }

    // Called on the Server when a Client disconnects from the Server.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("On dissconnect");
        base.OnServerDisconnect(conn);
        // The code in here does not run when a Client disconnects from the Server.
        
    }

}


/*
public class TeamCounter : NetworkManager {













    NetworkManager netMan;
    public static int numberOfTeams = 0;

	// Use this for initialization
	void Start ()
    {
        netMan = NetworkManager.singleton;

    }
	
	// Update is called once per frame
	void Update ()
    {
        this.ser
        /*
        if (isServer)
        {

            
            if (netMan)
            {
                Debug.Log("# = " + netMan.numPlayers);
            }
            else
            {
                Debug.Log("cannot find network namager");
            }
     }
  
    }

    void FindAllPlayers()
    {
        GameObject[] playerList;
        playerList = GameObject.FindGameObjectsWithTag("Player");

        ArrayList team = new ArrayList();

        foreach (GameObject obj in playerList)
        {
            UnitOwner owner = obj.GetComponent<UnitOwner>();
            if (owner)
            {
                team.Add(owner);
            }
        }

        
        

    }
       


   

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("On COnnect");
        // The code in here runs fine when a Client connects to the Server.

        //NetworkManager.singleton.
        
    }

    // Called on the Server when a Client disconnects from the Server.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("On dissconnect");
        // The code in here does not run when a Client disconnects from the Server.
    }
   

}
*/

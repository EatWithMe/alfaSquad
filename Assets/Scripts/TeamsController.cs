﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TeamsController : NetworkBehaviour {

    [SyncVar]
    public int numberOfTeams = 2; //-1 ,0 , 1 ; -1 = hostile to all - we have 3 teams

    [SyncVar]
    public SyncListInt numberOfPlayers = new SyncListInt();

    private ArrayList teamsArray; // array of arrays

    
	// Use this for initialization
	void Start ()
    {
        if (isServer)
        {
            reSizeTeamArray();
            //LinkToNetworkManager();
        }
    }
	

	// Update is called once per frame
	void Update ()
    {
	
	}

    
    void LinkToNetworkManager()
    {
        GameObject netMngObj = GameObject.FindGameObjectWithTag("myNetworkManager");
        if (netMngObj != null)
        {
            MyNetworkManager netMng = netMngObj.GetComponent<MyNetworkManager>();
            netMng.LinkWithTeamCtrl(this);
        }
        else
        {
            Debug.Log("Cannot find gameobject teamCotlooer;");
        }

    }


    [Server]
    void SetNumberOfTeams(int newTeamNumber)
    {
        numberOfTeams = newTeamNumber;
    }

    [Server]
    void reSizeTeamArray()
    {

        Debug.Log("reSizeTeamArray");
        teamsArray = new ArrayList();


        for (int i = 0; i <= (numberOfTeams -1 + 1 ); i++)
        {
            ArrayList teamArray = new ArrayList();
            teamsArray.Add(teamArray);
            numberOfPlayers.Add(0);
        }
    }


    void FillListWithCurrentPlayers()
    {
        GameObject[] playerList;
        playerList = GameObject.FindGameObjectsWithTag("Player");

        ArrayList team = new ArrayList();

        foreach (GameObject obj in playerList)
        {
            UnitOwner owner = obj.GetComponent<UnitOwner>();
            if (owner)
            {
                if (owner.teamIndex >= 0)
                {
                    ((ArrayList)teamsArray[owner.teamIndex]).Add(obj);
                }

            }
        }
    }    

    public void RegisterPlayerSquadInTeam(GameObject squad)
    {
        UnitOwner owner = squad.GetComponent<UnitOwner>();
        if (owner)
        {
            int teamIndex = owner.teamIndex +1 ; // +1 because -1 team is agains all
            if (teamIndex <= numberOfTeams)
            {
                ((ArrayList)teamsArray[owner.teamIndex]).Add(squad);
            }
            else
            {
                Debug.LogError("Wronge teamIndex. It is More than normal = " + teamIndex + "from  = " + numberOfTeams);
            }
        }
        else
        {
            Debug.LogError("Fail to register playerSquad no owner");
        }
    }



    /// <summary>
    /// check all objects // called but network manager
    /// </summary>
    [Server]
    public void RemoveDeadPlayerObjects()
    {
        for ( int teamIndex = 0; teamIndex <= (numberOfTeams - 1 + 1) ; teamIndex++)
        {
            ArrayList team = ((ArrayList)teamsArray[teamIndex]);
            for (int i = ( team.Count - 1) ; i >=0 ; i--)
            {
                GameObject playerSquad = (GameObject)team[i];

                if (playerSquad == null)
                {
                    team.RemoveAt(i);
                }
            }
        }
    }

    public int GetNumberOfPlayersInTheTeam(int teamIndex)
    {
        int res = -1;

        if (teamIndex <= numberOfTeams)
        {
            res = numberOfPlayers[teamIndex + 1]; // -1 0 1 ; +1
        }

        return res;
    }

    
 }
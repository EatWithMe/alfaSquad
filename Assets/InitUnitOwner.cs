using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class InitUnitOwner : NetworkBehaviour {

    private bool showGui = true;
    private int teamNumber = 0;

    // Use this for initialization
    void Awake()
    {
        UnitOwner owner = this.GetComponent<UnitOwner>();

        owner.playerName = "lalalal";
        owner.teamIndex = -1;
        owner.playerNetId = this.netId;

        InitTeamsNumber();


    }

    void InitTeamsNumber()
    {
        GameObject obj =  GameObject.FindGameObjectWithTag("TeamsController");
        if (obj)
        {
            TeamsController tc = obj.GetComponent<TeamsController>();
            if (tc)
            {
                teamNumber = tc.numberOfTeams;
            }
        }
    }

    void OnGUI()
    {
        if (showGui)
        {

        }
    }
}

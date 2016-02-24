using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class InitUnitOwner : NetworkBehaviour {

    

    void Start()
    {
       

        if (isLocalPlayer)
        {

            CmdInitOwnership("playerName", 0);

            /*
            Debug.Log("owner.playerNetId = ISSERVER" + this.netId);

            owner.playerName = "lalalal";
            owner.teamIndex = 0; // 0 - free for all; 
            owner.playerNetId = this.netId;
            */
        }

    }


    [Command]
    void CmdInitOwnership(string playerName, int teamIndex)
    {
        UnitOwner owner;
        owner = this.GetComponent<UnitOwner>();

        owner.playerName = playerName;
        owner.teamIndex = teamIndex; // 0 - free for all; 
        owner.playerNetId = this.netId;

        //we dont need to do RcpClient becouse all those fields are [Syncvar]
    }
}

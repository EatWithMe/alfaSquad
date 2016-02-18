using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class InitUnitOwner : NetworkBehaviour {

    // Use this for initialization
    void Awake()
    {
        UnitOwner owner = this.GetComponent<UnitOwner>();

        owner.playerName = "lalalal";
        owner.teamIndex = -1;
        owner.playerNetId = this.netId;
        
    }

}

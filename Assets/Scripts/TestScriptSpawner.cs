using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class TestScriptSpawner : NetworkBehaviour {

    public GameObject unitPrefub;
    public GameObject unit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    [Client]
	void Update ()
    {

        if (isLocalPlayer)

        {
            Debug.Log("ISLOCAL");
            if (Input.GetMouseButtonDown(0))
            {
                if (unit == null) CmdSpawnUnit();
            }
        }

        
        
    }

    [Command]
    void CmdSpawnUnit()
    {
        GameObject obj;
        obj = Instantiate(unitPrefub, transform.position, transform.rotation) as GameObject;
        //obj.transform.parent = this.transform;

        //obj.parentNetId = this.netId;
        //obj.SendMessage("MsgSetPanetNetId",this.netId);

        obj.transform.parent = this.transform; //Set the parent transform on the server

        NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
        
        
    }


    //[Command]
    void CmdMoveChild(int dir)
    {
        //RpcMoveChild(dir);
        unit.transform.Translate(Vector3.left * dir);
    }


    //[ClientRpc]
    void RpcMoveChild(int dir)
    {

        Debug.Log("Movce command "+ dir) ;
        unit.transform.Translate(Vector3.left * dir);
    }

}

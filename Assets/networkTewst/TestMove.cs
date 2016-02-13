using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TestMove : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (hasAuthority)
        {
            if (Input.GetKey("w"))
            {
               CmdMoveChild(-10);
            }
            else if (Input.GetKey("s"))
            {
                CmdMoveChild(10);
            }
        }
    }


    [Command]
    void CmdMoveChild(int dir)
    {
        //RpcMoveChild(dir);
        transform.Translate(Vector3.left * dir * Time.deltaTime);
    }

}

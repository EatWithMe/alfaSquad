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
            if (Input.GetKeyDown("w"))
            {
               MoveChild(-1);
            }
            else if (Input.GetKeyDown("s"))
            {
                MoveChild(1);
            }
        }
    }


    //[Command]
    void MoveChild(int dir)
    {
        //RpcMoveChild(dir);
        transform.Translate(Vector3.left * dir);
    }

}

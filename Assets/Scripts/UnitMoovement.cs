using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMoovement : NetworkBehaviour {

    private Vector3 target;
    private NavMeshAgent agent;

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdSetTarget(Vector3 targetPoint)
    {
        if (isServer)
        {
            
            if (agent.SetDestination(targetPoint))
            {
                target = targetPoint;
                RpcSetTarget(targetPoint);
            }
        }
    }

    [ClientRpc]
    void RpcSetTarget(Vector3 targetPoint)
    {
        if (agent.SetDestination(targetPoint))
        {
            target = targetPoint;
        }
    }

}

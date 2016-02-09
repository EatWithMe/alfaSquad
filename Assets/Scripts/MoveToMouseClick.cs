using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveToMouseClick : NetworkBehaviour {


    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


	// Update is called once per frame
	void Update ()
    {
        //if (isLocalPlayer)
        if (isLocalPlayer)
        {
            Debug.Log("Nove to click Is hasAuthority");
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 hitPos;
                if (GetMouseHitPosition(out hitPos))
                {
                    //target = tmpTarget;
                    //agent.SetDestination(hitPos);
                    CmdSetDestanation(hitPos);
                    
                }
            }
            else
            {
                return;
            }
        }
	}

    [Command]
    void CmdSetDestanation(Vector3 hitPos)
    {
        agent.SetDestination(hitPos);
    }


    float GetPathLenght()
    {
        return -1;
    }


    bool GetMouseHitPosition(out Vector3 hitPos)
    {
        bool res = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitPos = hit.point;
            res = true;
        }
        else
        {
            hitPos = new Vector3(0, 0, 0);
            //res = false;
        }

        return res;
    }
}

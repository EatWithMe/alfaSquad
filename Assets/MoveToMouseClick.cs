using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveToMouseClick : MonoBehaviour {


    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetMouseButtonDown(1) )
        {
            Vector3 hitPos;
            if (GetMouseHitPosition(out hitPos))
            {
                //target = tmpTarget;
                agent.SetDestination(hitPos);
                
            }
        }
        else
        {
            return;
        }
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

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMoovement : MonoBehaviour {

    private Vector3 target;
    private NavMeshAgent agent;

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTarget(Vector3 targetPoint)
    {
        if ( agent.SetDestination(targetPoint) )
        {
            target = targetPoint;
        }
    }

}

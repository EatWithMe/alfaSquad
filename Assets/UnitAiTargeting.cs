using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class UnitAiTargeting : MonoBehaviour {

    public float rotationSpeedMax = 200f;
    public GameObject target;

    private float targetSearchAllowedFromTime;
    private float targetSearchDelay = 1f; // we can serarch only 1 per sec

    private UnitOwner owner;

    // Use this for initialization
    void Start ()
    {
        owner = GetComponent<UnitOwner>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        //to shoot we need to turn towords target
        turnTowardsTarget();
    }

    void FixedUpdate()
    {

    }


    void turnTowardsTarget()
    {
        if ( target != null)
        {
            if (turnTowardsPoint(target.transform.position))
            {
                //SendWeCanShoot(true);
            }
            else
            {
                //SendWeCanShoot(false);
            }
        }
        else
        {
            //we can  search targets only 1 per time interwal
            if (Time.time > targetSearchAllowedFromTime  )
            {
                // we need to find new target
                findNewTarget();
                targetSearchAllowedFromTime += targetSearchDelay;
            }
        }
    }

    void findNewTarget()
    {
        float searchRadius = 100;
        GameObject closestTarget = null;
        float closestDistance = 10000f;

        Collider[] objAroundUs = Physics.OverlapSphere(transform.position, searchRadius);

        if (objAroundUs.Length > 0 )
        {
            foreach (Collider col in objAroundUs)
            {
                if (col.tag == "Unit")
                {

                    if (this.gameObject == col.gameObject) continue;

                    UnitOwner colOwner = col.GetComponent<UnitOwner>();
                    if (colOwner == null) continue;

                    

                    if (! UnitOwner.isFriendly(owner, colOwner) ) //we need to check eney or not
                    {
                        if (closestTarget == null)
                        {
                            closestTarget = col.gameObject;
                            closestDistance = Vector3.Magnitude(transform.position - closestTarget.transform.position);
                        }
                        else
                        {
                            //for case of 2 colliders per object
                            if (col.gameObject != closestTarget)
                            {
                                float distance = Vector3.Magnitude(transform.position - col.transform.position);
                                if (distance < closestDistance)
                                {
                                    closestTarget = col.gameObject;
                                    closestDistance = distance;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (closestTarget != null) target = closestTarget;

    }



    /// <summary>
    /// ture - if we already faced to target;
    /// </summary>
    /// <param name="RotateToTarget"></param>
    /// <returns></returns>
    bool turnTowardsPoint(Vector3 RotateToTarget)
    {

        bool weAreLookingAtTarget = false;
        Vector3 diff = RotateToTarget - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, rot_z, 0f), rotationSpeedMax * Time.deltaTime);
        //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0f, rot_z , 0f), rotationSpeedMax * Time.deltaTime);

        //---------a we already aimed
        float angle = Vector3.Angle(transform.rotation * Vector3.forward, diff);
        float distance = Vector3.SqrMagnitude(diff);
        if ( ( distance * angle ) < 1.3) weAreLookingAtTarget = true; //some magic koeff here
        //---------a we already aimed

        return weAreLookingAtTarget;
    }

}

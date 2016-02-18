using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class UnitAiTargeting : NetworkBehaviour {

    public float rotationSpeedMax = 200f;
    public GameObject target;
    public const float autoTargetMaxDistance = 100;

    private float targetSearchAllowedFromTime;
    private float targetSearchDelay = 0.5f; // we can serarch only 1 per sec

    private float nextcheckTargetDistanceTime = 0;
    private float checkTargetDistanceTimeDealay = 1f; // we will check current target only 1 per n sec

    private UnitOwner owner;

    private bool weaponTriggerState = false;


    // Use this for initialization
    void Start ()
    {
        //if (hasAuthority)
        //{
            owner = GetComponent<UnitOwner>();
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        //to shoot we need to turn towords target
        turnTowardsTarget();

        if (hasAuthority)
        {
            checkTargetForValidable();
        }
    }

    void FixedUpdate()
    {

    }

    /// <summary>
    /// if target is too far away - we will releace the target
    /// if target is behind the wall - we will releace target
    /// </summary>
    void checkTargetForValidable()
    {
        if (target != null)
        {
            if (Time.time > nextcheckTargetDistanceTime)
            {

                if ( targetIsShootable(target) )
                {
                    checkTargetForDistance();
                }
                else
                {
                    //if we cannot whot the target we will try to find new one
                    setNewTarget(null);
                    findNewTarget();
                }


                nextcheckTargetDistanceTime += checkTargetDistanceTimeDealay;
            }
        }
    }

    /// <summary>
    /// check for outrange and another target that are extreamly close to us
    /// </summary>
    void checkTargetForDistance()
    {
        if ( target != null)
        {
            float distance = Vector3.SqrMagnitude(target.transform.position - this.transform.position);
            float uselessDistance = autoTargetMaxDistance * 1.15f;

            if (distance > uselessDistance)
            {
                findNewTarget();
            }
            else 
            {
                float retargetDistance = distance * 0.5f; //if we have turgets close that 50% ot this
                findNewTarget(retargetDistance);
            }

        }
    }


    /// <summary>
    /// check if unit can the the had of target unit
    /// </summary>
    /// <param name="trg"></param>
    /// <returns></returns>
    bool targetIsShootable(GameObject trg)
    {
        bool res = false;

        if (trg != null)
        {
            //we need to raycast from weapon point to target head

            //todo add dimamic head coords detection
            Vector3 headPos = new Vector3(0, 0.075f, 0);///0.075 = 50 % body height(0.1) + 50 % head (0.05) //50% becouse of collider pivot at the middle
            Vector3 outHeadPos = this.transform.position + headPos;
            Vector3 targetHeadPos = trg.transform.position + headPos;

            //Debug.DrawLine(outHeadPos, targetHeadPos, Color.blue, 32f, false);

            RaycastHit hit;
            if (Physics.Raycast(outHeadPos, targetHeadPos - outHeadPos, out hit))
            {
                //Debug.DrawLine(outHeadPos, targetHeadPos, Color.red,32f, false);

                
                if (hit.collider.gameObject == trg)
                {
                    //Debug.Log("Turget In sign");
                    res = true;
                }
                else
                {
                    //
                    //Debug.Log("Turget out of sign");
                    //Debug.Log(" hit.collider.name = [" + hit.collider.name + "] target = [" + trg.name);
                }


            }
            else
            {
                //Debug.Log("targetIsShootable HIT HONIGT");
            }
        }


        return res;
    }



    void turnTowardsTarget()
    {
        if ( target != null)
        {
            //all instanses turring
            bool weAreTurnedToTarget = turnTowardsPoint(target.transform.position) ;

            //but does not press trigger
            if (hasAuthority)
            {
                if (weAreTurnedToTarget)
                {
                    PushTheWeaponTrigger(true);
                }
                else
                {
                    PushTheWeaponTrigger(false);
                }
            }
        }
        else
        {
            if (hasAuthority)
            {
                PushTheWeaponTrigger(false);

                //we can  search targets only 1 per time interwal
                if (Time.time > targetSearchAllowedFromTime)
                {
                    // we need to find new target
                    findNewTarget();
                    targetSearchAllowedFromTime += targetSearchDelay;
                }
            }
        }
    }

    void PushTheWeaponTrigger(bool val)
    {
        if (! ( weaponTriggerState == val) )
        {
            weaponTriggerState = val;
            //SendMessage("WeaponTriggerSetToFire", true);
            BroadcastMessage("WeaponTriggerSetToFire", val);
        }
    }
    

    void findNewTarget(float radius = autoTargetMaxDistance)
    {
        
        GameObject closestTarget = null;
        float closestDistance = 1000f;


        Collider[] objAroundUs = Physics.OverlapSphere(transform.position, radius);
        // BUG - this shit is not workink  - layer does not applied
        //Collider[] objAroundUs = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.NameToLayer("Units") );

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

                        if (targetIsShootable(col.gameObject))
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
        }

        if (closestTarget != null) setNewTarget(closestTarget);

    }

    void setNewTarget(GameObject newTarget)
    {

        //target = newTarget; // commended - because we will recieave turget from server


        //if we set new target - then retargeting can only be allowed in delay
        nextcheckTargetDistanceTime = Time.time + checkTargetDistanceTimeDealay;

        if (newTarget != null)
        {
            NetworkIdentity ni = newTarget.GetComponent<NetworkIdentity>();
            if (ni != null)
            {
                CmdSetNewTarget(ni.netId);
            }
            else
            {
                Debug.LogError("New target does not have netId. Item Name = [" + newTarget.name + "]");
            }
        }
        else
        {
            CmdResetTarget();
        }
    }

    [Command]
    void CmdResetTarget()
    {
        target = null;
        RpcResetTarget();
    }

    [ClientRpc]
    void RpcResetTarget()
    {
        target = null;
    }


    [Command]
    void CmdSetNewTarget (NetworkInstanceId id)
    {
        target = ClientScene.FindLocalObject(id);
        RpcSetNewTarget(id);
    }


    [ClientRpc]
    void RpcSetNewTarget(NetworkInstanceId id)
    {
        target = ClientScene.FindLocalObject(id);
    }


    /// <summary>
    /// </summary>
    /// <param name="RotateToTarget"></param>
    /// <returns>true - if we already faced to target;</returns>
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

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BulletMovement : NetworkBehaviour {

    public int moveSpeed = 20;
    public int lifeTimeSec = 5;
    public int bulletDamage = 30;

    private static float randomDamageRange = 0.2f ; // in %
    

    void Start()
    {
        Destroy(gameObject, lifeTimeSec);
    }

	// Update is called once per frame
	void Update ()
    {
        //MoveBulletForward();
    }

    void FixedUpdate()
    {
        CheckRayCastCollision();
        MoveBulletForwardFixedUpdate();
        
    }

    void MoveBulletForward()
    {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
    }

    void MoveBulletForwardFixedUpdate()
    {
        transform.Translate(Vector3.up * Time.fixedDeltaTime * moveSpeed);
    }

    void CheckRayCastCollision()
    {
        RaycastHit hit;
        float nextMoveDistance = moveSpeed * Time.fixedDeltaTime;

        if (Physics.Raycast(transform.position, transform.up , out hit, nextMoveDistance))
        {
            if (hit.transform)
            {
                //bullet calculate only at client side
                if (isServer)  DoDamageToHitObject(hit.collider);
                DestroyBullet();
                return;
            }
        }
    }

    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }


    /*
        void OnCollisionEnter(Collision col)
        {
           Destroy(this.gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            //GetComponent<Collider>().name
            //Debug.Log("OnTriggerEnter coll name = " + other.name );


        }

        */


    void DoDamageToHitObject(Collider other)
    {
        Damage dmg;
        dmg.amount = GenerateRandomDamage( bulletDamage );
        dmg.ownderNetId = GetComponent<UnitOwner>().playerNetId;

        other.gameObject.SendMessage("TakeDamage",dmg);
        DestroyBullet();

    }

    static float GenerateRandomDamage(float dmg)
    {
        float range = dmg * randomDamageRange;
        return dmg + Random.Range(-1 * range, range);
    }


}

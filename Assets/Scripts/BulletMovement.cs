using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BulletMovement : NetworkBehaviour {

    public int moveSpeed = 20;
    public int lifeTimeSec = 5;
    public int bulletDamage = 30;

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
                if (hasAuthority)  DoDamageToHitObject(hit.collider);
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
        //Debug.Log("we hit  = " + other.name);

        //if we hit something and this compounent have lifes 
        LifeStats victim = other.gameObject.GetComponent<LifeStats>();
        if (victim != null)
        {
            //Debug.Log("ifestats is FOUND!!!");
            victim.CmdTakeDamage(bulletDamage);
        }
        else
        {
            //Debug.Log("Cannot fine lifestats");
        }

        DestroyBullet();

    }

}

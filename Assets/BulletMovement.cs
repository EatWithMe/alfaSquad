using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

    public int moveSpeed = 20;
    public int lifeTimeSec = 5;
    public int bulletDamage = 30;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        Destroy(gameObject, lifeTimeSec);
	}


    void OnCollisionEnter(Collision col)
    {
        Debug.Log("coll name = " + col.gameObject.name);
       Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        //GetComponent<Collider>().name
        //Debug.Log("OnTriggerEnter coll name = " + other.name );
        

        //if we hit something and this compounent have lifes 
        LifeStats victim =   other.gameObject.GetComponent<LifeStats>();
        if (victim != null)
        {
            //Debug.Log("ifestats is FOUND!!!");
            victim.takeDamage(bulletDamage);
        }
        else
        {
            //Debug.Log("Cannot fine lifestats");
        }

        Destroy(this.gameObject);
    }

}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LifeStats))]
public class DamageTaker : MonoBehaviour  {


    private LifeStats stats;
    public GameObject damageTextPrefub;

	// Use this for initialization
	void Start ()
    {
        stats = GetComponent<LifeStats>();
        stats.OnDeath += OnMyDeath;

    }

    public void OnMyDeath()
    {
        //todo add death animation
        Destroy(this.gameObject );
    }


    public void OnMyDamage(float amount)
    {
        if ( damageTextPrefub != null)
        {
            Instantiate(damageTextPrefub, this.transform.position, this.transform.rotation);
        }
    }



    // it will alwais trigger because of ground collision - so we dont wee to use it
    //void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("Unit OnCollisionEnter " + col.gameObject.name);
    //   //Destroy(this.gameObject);
    //}

}

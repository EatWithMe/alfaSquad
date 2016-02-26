using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LifeStats))]
public class DamageTaker : MonoBehaviour  {


    private LifeStats stats;
    public GameObject damageTextPrefub;
    private GameObject squadControler;

	// Use this for initialization
	void Start ()
    {
        stats = GetComponent<LifeStats>();
        stats.OnDeath += OnMyDeath;
        stats.OnDamage += OnMyDamage;

    }

    public void OnMyDeath()
    {
        //todo add death animation
        ReportToSquadAboutMyDeath();
        Destroy(this.gameObject );
    }

    void ReportToSquadAboutMyDeath()
    {

        if (squadControler != null)
        {
            squadControler.SendMessage("UnitIsDead", this.gameObject);
        }
    }
        


    public void OnMyDamage(float amount)
    {
        if ( damageTextPrefub != null)
        {
            

            Quaternion rot = Quaternion.Euler(90,0,0);
            GameObject pop;
            //pop = Instantiate(damageTextPrefub, this.transform.position,  this.transform.rotation) as GameObject;
            pop = Instantiate(damageTextPrefub, this.transform.position, rot) as GameObject;
            FloatingText flText;
            flText = pop.GetComponent<FloatingText>();
            if (flText != null)
            {
                flText.SetFloatingText(amount.ToString("#."), Color.red);
            }
        }

    }




    public void SetSquadControler(GameObject squad)
    {
        squadControler = squad;
    }

    // it will alwais trigger because of ground collision - so we dont wee to use it
    //void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log("Unit OnCollisionEnter " + col.gameObject.name);
    //   //Destroy(this.gameObject);
    //}

    /// <summary>
    /// this function called buy victim - he reports about damage
    /// </summary>
    /// <param name="dmg"></param>
    public void DamageReportCallBack (Damage dmg)
    {
        
    }

}

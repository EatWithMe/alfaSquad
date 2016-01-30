using UnityEngine;
using System.Collections;

public class LifeStats : MonoBehaviour
{

    public int      healthMax = 100;
    [SerializeField]
    private float   healthCurrent = 100;
    public float    healthRegenPerSec = 0.1f;



    //public delegate void OnDeathAction(GameObject enemy);

    public delegate void OnDeathAction();
    public OnDeathAction OnDeath;

    public delegate void OnDamageAction(float amount);
    public OnDamageAction OnDamage;

    // Use this for initialization
    void Start()
    {
        healthCurrent = healthMax;
    }

    void Update()
    {
        regenerateHealth();
    }

    
    void regenerateHealth()
    {
        if (  ( healthCurrent  < healthMax) && (healthCurrent > 0) ) 
        {
            healthCurrent += Time.deltaTime * healthRegenPerSec;
            if (healthCurrent > healthMax) healthCurrent = healthMax;
        }
    }
    

    public void takeDamage(float amount)
    {
        healthCurrent -= amount;

        deathReport();
    }

    public void takeHeal(float amount)
    {
        healthCurrent += amount;
        if (healthCurrent > healthMax) healthCurrent = healthMax;
    }

    void damageReport (float amount)
    {
        if (OnDamage!= null)
        {
            OnDamage(amount);
        }
    }


    
    void deathReport()
    {
        if (healthCurrent <= 0)
        {
            if (OnDeath != null)
            {
                // Announce enemy death to subscibers
                OnDeath();
            }
        }
    }
}

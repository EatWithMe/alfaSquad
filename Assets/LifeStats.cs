using UnityEngine;
using System.Collections;

public class LifeStats : MonoBehaviour
{


    private int _healthMax = 100;

    [SerializeField]
    private float   _healthCurrent = 100;
    public float    healthRegenPerSec = 0.1f;

    

    //public delegate void OnDeathAction(GameObject enemy);

    public delegate void OnDeathAction();
    public OnDeathAction OnDeath;

    public delegate void OnDamageAction(float amount);
    public OnDamageAction OnDamage;

    // if some stats or hp is changed
    public delegate void OnStatsChanged();
    public OnStatsChanged OnStats;


    public int healthMax
    {
        get { return _healthMax; }
        set
        {
            _healthMax = value;
            riseOnStatsEvent();
        }
    }

    private float previousOnStatHealth = 0f;

    public float healthCurrent
    {
        get  { return _healthCurrent; }
        set
        {
            _healthCurrent = value;
            if ( Mathf.Abs (_healthCurrent - previousOnStatHealth)  > 0.9)
            {
                previousOnStatHealth = _healthCurrent;
                riseOnStatsEvent(); //we will send event only of 0.9% of hp update
            }
        }
    }



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

    void riseOnDamagEvent (float amount)
    {
        if (OnDamage!= null) OnDamage(amount);
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

    void riseOnStatsEvent()
    {
        if (OnStats != null) OnStats();
    }
}

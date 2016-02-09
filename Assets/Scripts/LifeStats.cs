using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LifeStats : NetworkBehaviour
{

    [SyncVar]
    private int _healthMax = 100;

    [SerializeField]
    [SyncVar]
    private float   _healthCurrent = 100;
    [SyncVar]
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
            RpcRiseOnStatsEvent();
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
                RpcRiseOnStatsEvent(); //we will send event only of 0.9% of hp update
            }
        }
    }



    // Use this for initialization
    void Start()
    {
        healthCurrent = healthMax;
    }

    [Server]
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
    
    [Command]
    public void CmdTakeDamage(float amount)
    {
        healthCurrent -= amount;
        RpcRiseOnDamagEvent(amount);
        deathReport();
    }

    public void takeHeal(float amount)
    {
        healthCurrent += amount;
        if (healthCurrent > healthMax) healthCurrent = healthMax;
    }

    [ClientRpc]
    void RpcRiseOnDamagEvent (float amount)
    {
        if (OnDamage!= null) OnDamage(amount);
    }


    
    void deathReport()
    {
        if (healthCurrent <= 0)
        {
            RpcDeathReport();
        }
    }

    [ClientRpc]
    public void RpcDeathReport()
    {
        if (OnDeath != null)
        {
            // Announce enemy death to subscibers
            OnDeath();
        }

    }

    [ClientRpc]
    void RpcRiseOnStatsEvent()
    {
        if (OnStats != null) OnStats();
    }
}

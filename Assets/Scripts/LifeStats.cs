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
        if (isServer)
        {
            healthCurrent = healthMax;
        }
    }

    
    void Update()
    {
        if (isServer)
        {
            regenerateHealth();
        }
    }

    
    void regenerateHealth()
    {
        if (  ( healthCurrent  < healthMax) && (healthCurrent > 0) ) 
        {
            healthCurrent += Time.deltaTime * healthRegenPerSec;
            if (healthCurrent > healthMax) healthCurrent = healthMax;
        }
    }


    
    //public void TakeDamage(float amount)
    [Server]
    public void TakeDamage(Damage dmg)
    {
        
        if (isServer)
        {
            healthCurrent -= dmg.amount;
            DeathReport();
            RpcTakeDamage(dmg.amount);


            dmg.ownderNetId = this.netId;
            ReportDamageOwnerAboutHit(dmg);
        }
        //else
        //{

            //CmdTakeDamage(amount);
            //return;
            
        //}
    }

    [Server]
    void ReportDamageOwnerAboutHit(Damage dmg)
    {
        GameObject bulletOwner = ClientScene.FindLocalObject(dmg.ownderNetId);
        if ( bulletOwner!= null)
        {
            bulletOwner.SendMessage("DamageReportCallBack", dmg);
        }
        else
        {
            Debug.Log("ReportDamageOwnerAboutHit: cannot find bullet owner");
        }
    }


    //[Command]
    //void CmdTakeDamage(float amount)
    //{
    //    TakeDamage(amount);
    //}

    [ClientRpc]
    void RpcTakeDamage(float amount)
    {
        if (OnDamage != null) OnDamage(amount);
    }


    public void takeHeal(float amount)
    {
        healthCurrent += amount;
        if (healthCurrent > healthMax) healthCurrent = healthMax;
    }



    void DeathReport()
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

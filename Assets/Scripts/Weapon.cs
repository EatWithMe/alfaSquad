﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Weapon : NetworkBehaviour {

    public GameObject weaponCurrent;
    //public GameObject[] weaponPrefubList;

    //private GameObject weaponList;
    private WeaponList weaponList;




    // Use this for initialization
    void Start () {
        InitWeaponList();
        //remote clients will have AUTHORITY at first iteration - when server will not have it
        if (hasAuthority)
        {
            SetDefaultWeapon();
        }

    }



    // Update is called once per frame
    void Update () {

        if ( hasAuthority)
        {
            NoEmptyWeaponForHoxtFIx();
        }   

    }

    void NoEmptyWeaponForHoxtFIx()
    {
        if (weaponCurrent == null)
        {

            SetDefaultWeapon();
        }
    }
    

    void InitWeaponList()
    {
        GameObject tmp;
        tmp = GameObject.FindGameObjectWithTag("WeaponList");

        if (tmp != null)
        {
            weaponList = tmp.GetComponent<WeaponList>();
            if ( weaponList == null )
            {
                Debug.LogError( "Cannot find Weapon List compounent" );
            }
        }
        else
        {
            Debug.LogError("Cannot find Weapon List object");
        }
    }


    public void SetDefaultWeapon()
    {

        if (weaponList.Length <= 0)
        {
            Debug.LogError("No WeaponPrefub is found");
            return;
        }
        else
        {
            //setDeafaultWeapon
            
            switchWeaponTo(0);
        }

    }

    public void switchWeaponTo(int index)
    {
        if ( index >= weaponList.Length)
        {
            Debug.LogError("Cannot switch weapon to [" + index + "] for object = " + this.gameObject.name);
        }
        else
        {

            GameObject weaponPref = weaponList.GetWeaponPrefub(index);
            if (weaponPref != null)
            {
                CmdCreateNewWeaponUnprotected(index);
            }
            else
            {
                Debug.LogError("Cannot switch weapon to [" + index + "]. prefub is empty. for object = " + this.gameObject.name);
            }
            
        }
    }

    [Command]
    public void CmdCreateNewWeaponUnprotected(int index)
    {
        if (weaponCurrent != null) Destroy(weaponCurrent);
        weaponCurrent = Instantiate(weaponList.GetWeaponPrefub(index), this.transform.position, this.transform.rotation) as GameObject;


        //UnitOwner thisUnit = new UnitOwner();
        //thisUnit.setOwnerShip(GetComponent<UnitOwner>());
        //thisUnit.playerNetId = this.netId;


        weaponCurrent.SendMessage("setOwnerShip", GetComponent<UnitOwner>()); // our owner is weapon owner
        weaponCurrent.GetComponent<UnitOwner>().playerNetId = this.netId; //we override weapon owner to us
        weaponCurrent.SendMessage("SetPanetNetId", this.netId);
        weaponCurrent.SendMessage("SetParentRegistratorName", "PickUpWeapon");

        weaponCurrent.transform.parent = this.transform;

        //NetworkServer.SpawnWithClientAuthority(weaponCurrent, this.connectionToClient);
        NetworkIdentity identity = this.GetComponent<NetworkIdentity>();
        NetworkServer.SpawnWithClientAuthority(weaponCurrent, identity.clientAuthorityOwner);
    }

    public void PickUpWeapon(GameObject wpn)
    {
        weaponCurrent = wpn;
        wpn.transform.parent = this.transform;
        wpn.transform.rotation = this.transform.rotation;

        //wpn.transform.localPosition = new Vector3(0, 0, 0);
    }
}

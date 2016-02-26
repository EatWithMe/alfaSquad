using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponList : MonoBehaviour {

    [SerializeField]
    private GameObject[] weaponPrefubs;


    void Start()
    {
        SelfPrefabRegistration();
    }


    void SelfPrefabRegistration()
    {
        MyRegistrator.NetworkPrefubsRegistration(weaponPrefubs);
    }




    public GameObject GetWeaponPrefub (int i)
    {
        if ( ( i < weaponPrefubs.Length ) && ( i>=0 ) ) 
        {
            return weaponPrefubs[i];
        }
        else
        {
            return null;
        }
    }

    public int Length
    {
        get { return weaponPrefubs.Length;  }
        
        //return WeaponPrefubs.Length;
    }

}

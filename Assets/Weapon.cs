using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public GameObject weaponCurrent;
    //public GameObject[] weaponPrefubList;

    //private GameObject weaponList;
    private WeaponList weaponList;




    // Use this for initialization
    void Start () {

       InitWeaponList();

       SetDefaultWeapon();
    }
	
	// Update is called once per frame
	void Update () {
	
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


    void SetDefaultWeapon()
    {

        if (weaponList.Length <= 0)
        {
            Debug.LogError("No WeaponPrefub is found");
            return;
        }
        else
        {
            weaponCurrent = Instantiate( weaponList.GetWeaponPrefub(0) , this.transform.position, this.transform.rotation) as GameObject;
            weaponCurrent.gameObject.transform.parent = this.transform;
        }

    }

    void switchWeaponTo(int index)
    {
        if ( index > weaponList.Length)
        {
            Debug.LogError("Cannot switch weapon to [" + index + "] for object = " + this.gameObject.name);
        }
        else
        {

            Destroy(weaponCurrent);
            weaponCurrent = weaponCurrent = Instantiate( weaponList.GetWeaponPrefub(index) , this.transform.position, this.transform.rotation) as GameObject;
        }
    }

}

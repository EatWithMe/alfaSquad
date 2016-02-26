using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class UnitsPrefubList : MonoBehaviour
{

    public GameObject[] unitPrefubs;


    void Start()
    {
        SelfPrefabRegistration();
    }


    void SelfPrefabRegistration()
    {

        MyRegistrator.NetworkPrefubsRegistration(unitPrefubs);
    }

}
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerObjectSpawner : NetworkBehaviour {

    public GameObject[] spawnList;

	// Use this for initialization
	void Start () 
    {
        SelfPrefabRegistration();

        if (isServer)
        {
            SpawnAllObjects();
        }
    }


    void SelfPrefabRegistration()
    {
        MyRegistrator.NetworkPrefubsRegistration(spawnList);
    }



    void SpawnAllObjects()
    {
        if ( spawnList.Length>0 )
        {
            foreach (GameObject prefub in spawnList)
            {
                if (prefub)
                {
                    GameObject obj;
                    obj = Instantiate(prefub, transform.position, transform.rotation) as GameObject;
                    if (obj) NetworkServer.Spawn(obj);
                }
            }
        }
    }

   
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MyEventMaster : NetworkBehaviour {

    public GameObject   cargoDropPrefub;
    public int          cargoDropInterwal = 20;

    

	// Use this for initialization
	void Start ()
    {
        if (isServer)
        {
            if (cargoDropPrefub != null) StartCoroutine( StartEventGeneration(cargoDropPrefub,cargoDropInterwal) );
        }
	}


    public override void PreStartClient()
    {
        RegisterNetPrefab();
    }


    // Update is called once per frame
    void Update ()
    {
	
	}

    void RegisterNetPrefab()
    {
        MyRegistrator.NetworkPrefubRegistration(cargoDropPrefub);
    }


    IEnumerator StartEventGeneration(GameObject eventPrefab, int spawnInterval)
    {

        do
        {
            yield return new WaitForSeconds(spawnInterval);

            Transform respLocation = NetworkManager.singleton.GetStartPosition();
            if (respLocation == null) respLocation = this.transform; //it is better then nothing

            GameObject obj = Instantiate(eventPrefab, respLocation.position, respLocation.rotation) as GameObject;
            NetworkServer.Spawn(obj);

        }
        while (true);
                

    }

}

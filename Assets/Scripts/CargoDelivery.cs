using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CargoDelivery : NetworkBehaviour {

    public GameObject flarePrefab;
    public GameObject deliveryPrefab;
    public float deleaveryTimeDelay = 60f ;
    private float deleaveryTime; //Time.Time even
    //private bool deliverySpawned = false;

    private float deliverySoundActivationTime;
    private bool soundAvtivated = false;

    private AudioSource audio;
    public AudioClip deliverySound;

    static bool prefubsRegistered = false;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        if (isServer) initTimer();
        spawnFlare();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isServer)
        {
            tryToSpawnDeleavery();
            tryToPlaySound();
        }
        
    }

    public override void PreStartClient()
    {
        base.PreStartClient();
        SelfPrefabRegistration();
    }

    void SelfPrefabRegistration()
    {
        if (prefubsRegistered == false)
        {
            MyRegistrator.NetworkPrefubRegistration(deliveryPrefab);

            //MyRegistrator.NetworkPrefubRegistration(flarePrefab); flare is not networkobject
            prefubsRegistered = true;
        }
    }

    void initTimer()
    {
        float soundLength = deliverySound.length;

        deleaveryTime = Time.time + deleaveryTimeDelay;
        if (deleaveryTimeDelay > soundLength)
        {
            deliverySoundActivationTime = deleaveryTime - soundLength;
        }
        else
        {
            deliverySoundActivationTime = Time.time; //activate  now;
        }
    }

    [Server]
    void tryToSpawnDeleavery()
    {
       if (Time.time > deleaveryTime)
       {
            Vector3 additionalHeight = new Vector3(0, 10, 0);
            GameObject obj = Instantiate(deliveryPrefab, this.transform.position + additionalHeight, this.transform.rotation ) as GameObject;
             
            NetworkServer.Spawn(obj);
            NetworkServer.Destroy(this.gameObject);
            //deliverySpawned = true;
        }
    }

    [Server]
    void tryToPlaySound()
    {
        if (!soundAvtivated)
        {
            if (Time.time > deliverySoundActivationTime)
            {
                //PlaySound;
                RpcPlayDeliverySound();
                soundAvtivated = true;
            }
        }
    }

    [ClientRpc]
    void RpcPlayDeliverySound()
    {
        audio.PlayOneShot(deliverySound);
    }

    //flare is not important - so we create it on client
    void spawnFlare()
    {
        Vector3 additionalHeight = new Vector3(0, 2, 0);
        GameObject flare;
        flare = Instantiate(flarePrefab, this.transform.position + additionalHeight, this.transform.rotation) as GameObject;
        Destroy(flare, deleaveryTimeDelay); //send command to destroy flare 
    }

}
 
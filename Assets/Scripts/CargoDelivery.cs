using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CargoDelivery : MonoBehaviour {

    public GameObject flarePrefab;
    public GameObject deliveryPrefab;
    public float deleaveryTimeDelay = 60f ;
    private float deleaveryTime; //Time.Time even
    //private bool deliverySpawned = false;

    private float deliverySoundActivationTime;
    private bool soundAvtivated = false;

    private AudioSource audio;
    public AudioClip deliverySound;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        initTimer();
        spawnFlare();
    }
	
	// Update is called once per frame
	void Update ()
    {

        tryToSpawnDeleavery();
        tryToPlaySound();
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

    void tryToSpawnDeleavery()
    {
            if (Time.time > deleaveryTime)
            {
                Vector3 additionalHeight = new Vector3(0, 10, 0);
                Instantiate(deliveryPrefab, this.transform.position + additionalHeight, this.transform.rotation );
                Destroy(this.gameObject);
                //deliverySpawned = true;
            }
    }

    void tryToPlaySound()
    {
        if (!soundAvtivated)
        {
            if (Time.time > deliverySoundActivationTime)
            {
                //PlaySound;
                audio.PlayOneShot(deliverySound);
                soundAvtivated = true;
            }
        }
    }


    void spawnFlare()
    {
        Vector3 additionalHeight = new Vector3(0, 2, 0);
        GameObject flare;
        flare = Instantiate(flarePrefab, this.transform.position + additionalHeight, this.transform.rotation) as GameObject;
        Destroy(flare, deleaveryTimeDelay); //send command to destroy flare 
    }

}
 
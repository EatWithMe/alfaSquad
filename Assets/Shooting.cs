using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public GameObject bulletPrefab;

    public AudioClip ShootSound;
    public AudioClip ReloadSound;

    //weapon stats
    public int clipSize = 5 ;
    public float shotsDelay = 1f;
    private float reloadTimeSeconds = 3f; //  ReloadSound.length

    //conesize at 10x distance
    public float accuracyMin = 1f;
    public float accuracyMax = 2f;
    public float accuracyPenaltyPerShot = 0.3f;
    public float accuracyDropPerSecond = 0.3f;

    private float currentAccuracy;
    private int currentAmmoAtClip;

    private float shootDelayEndTime = 0f;
    private float reloadEndTime = 0f;
    private bool reloadStarted; // flag indicates that reload was started

    private Transform FirePoint;
    private AudioSource audio;

    void Start()
    {
        currentAccuracy = accuracyMax;
        currentAmmoAtClip = clipSize;
        reloadTimeSeconds = ReloadSound.length;
        //Debug.Log("Reload time = " + reloadTimeSeconds);

        FirePoint = transform.Find("bulletStartPosition");
        audio = GetComponent<AudioSource>();


    }


    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetMouseButton(0) )
        {
            if ( IsReloading() )
            {
                
            }
            else
            {
                if ( reloadStarted == true )
                {
                    FillClipWithAmmo();
                }

                TryToShoot();
            }
        }
	
	}

    void TryToShoot()
    {
        if (!isShootOnDelay())
        {
            if (currentAmmoAtClip > 0)
            {
                ShootABullet();
                currentAmmoAtClip--;
            }
            else
            {
                StartReloading();
            }
        }
    }

    bool isShootOnDelay()
    {
        bool res = (Time.time >= shootDelayEndTime )? false: true;
        return res;
    }

    bool IsReloading()
    {
        bool res = ( Time.time >= reloadEndTime) ? false: true;
        return res;
    }

    void StartReloading()
    {
        reloadEndTime = Time.time + reloadTimeSeconds;
        reloadStarted = true;
        PlaySound(ReloadSound);
    }

    void FillClipWithAmmo()
    {
        currentAmmoAtClip = clipSize;
        reloadStarted = false;
    }

/*
    IEnumerator PlaySound(AudioClip sound)
    {

        Debug.Log("PlaySound = "+ sound.name);
        

        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = sound;
        audio.Play();
    }
    */


    void PlaySound(AudioClip sound)
    {
        audio.PlayOneShot(sound,Random.Range(0.5f, 1f) ) ;
    }


    void ShootABullet()
    {
        shootDelayEndTime = Time.time + shotsDelay;
        SomeEffect();
        PlaySound(ShootSound);
    }

    void SomeEffect()
    {
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
    }

}


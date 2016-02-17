using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponTemplate : NetworkBehaviour
{
    

    public GameObject bulletPrefab;
    public Transform FirePoint;

    public AudioClip ShootSound;
    public AudioClip ReloadSound;

    public int weaponCost = 1;

    //weapon stats
    public int damage = 30;
    public int clipSize = 5;
    public float shotsDelay = 1f;
    private float reloadTimeSeconds = 3f; //  ReloadSound.length

    //degrees spray semi circle
    public float accuracyMin = 6f;
    public float accuracyMax = 20f;
    public float accuracyPenaltyPerShot = 4f;
    public float accuracyDropPerSecond = 2f;

    public float optimalDistance = 100f;


    
    private float currentAccuracy;
    private int currentAmmoAtClip;

    private float shootDelayEndTime = 0f;
    private float reloadEndTime = 0f;
    private bool reloadStarted; // flag indicates that reload was started

    private bool triggerIsPressed = false;
    
    private AudioSource audio;

    void Start()
    {
        currentAccuracy = accuracyMax;
        currentAmmoAtClip = clipSize;
        reloadTimeSeconds = ReloadSound.length;
        //Debug.Log("Reload time = " + reloadTimeSeconds);

        FirePoint = transform.Find("bulletStartPosition");
        audio = GetComponent<AudioSource>();

        CalculateOptimalDistance();

    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(0))
        if ( triggerIsPressed ) 
        {
            if (IsReloading())
            {

            }
            else
            {
                if (reloadStarted == true)
                {
                    FillClipWithAmmo();
                }

                TryToShoot();
            }
        }

        DecreaseAccuracy();

    }

    void WeaponTriggerSetToFire(bool val)
    {
        triggerIsPressed = val;
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
        bool res = (Time.time >= shootDelayEndTime) ? false : true;
        return res;
    }

    bool IsReloading()
    {
        bool res = (Time.time >= reloadEndTime) ? false : true;
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
        audio.PlayOneShot(sound, Random.Range(0.5f, 1f));
    }


    void ShootABullet()
    {
        shootDelayEndTime = Time.time + shotsDelay;
        CmdCreateBullet();
        PlaySound(ShootSound);
        IncreaseAccuracy();
    }

    [Command]
    void CmdCreateBullet()
    {
        GameObject bulletTmp;
        Quaternion bullerRotation = BulletRandomAccuracy(FirePoint.rotation);

        bulletTmp = Instantiate(bulletPrefab, FirePoint.position, bullerRotation) as GameObject;
        bulletTmp.SendMessage("setOwnerShip", GetComponent<UnitOwner>());
        bulletTmp.SendMessage("SetNetStartingRotation", bullerRotation);

        //NetworkServer.SpawnWithClientAuthority(bulletTmp, this.connectionToClient);

        //we cannot use client autority couse i cannot GiveDame to non aothorised object
        //NetworkIdentity identity = this.GetComponent<NetworkIdentity>();
        //NetworkServer.SpawnWithClientAuthority(bulletTmp, identity.clientAuthorityOwner);

        NetworkServer.Spawn(bulletTmp);
    }

    Quaternion BulletRandomAccuracy(Quaternion gunDirection)
    {

        Quaternion res = Quaternion.Euler(0, Random.Range(-1 * currentAccuracy, currentAccuracy), Random.Range(currentAccuracy, -1 * currentAccuracy)); ;
        res = res * gunDirection;
        return res;
    }

    void IncreaseAccuracy()
    {
        currentAccuracy += accuracyPenaltyPerShot;
        if (currentAccuracy > accuracyMax) currentAccuracy = accuracyMax;
    }

    void DecreaseAccuracy()
    {

        if (currentAccuracy > accuracyMin)
        {
            currentAccuracy -= (accuracyDropPerSecond * Time.deltaTime);
            if (currentAccuracy < accuracyMin) currentAccuracy = accuracyMin;
        }
    }

    /// <summary>
    /// find distance where we have some chance to hit target with minimal accuracy
    /// </summary>
    void CalculateOptimalDistance()
    {
        float MAX_ALLOUWED_DISTANCE = 100f;
        float MIN_ALLOUWED_DISTANCE  = 100f;
        float MINIMAL_CHANSE_TO_HIT = 0.05f;

        // that is body hit surface - taken from collision
        float bodySquare = 1f * 0.1f   + 0.2f * 0.05f ;
        float R = Mathf.Sqrt(bodySquare / (MINIMAL_CHANSE_TO_HIT * 3.14f));
        float X = R / Mathf.Tan(  ( 3.14f * accuracyMin) / 180f  );

        if ((X > MIN_ALLOUWED_DISTANCE) && (X < MAX_ALLOUWED_DISTANCE))
        {
            optimalDistance = X;
        }
        else
        {
            optimalDistance = ( MAX_ALLOUWED_DISTANCE + MIN_ALLOUWED_DISTANCE ) / 2;
        }
        


    }

}


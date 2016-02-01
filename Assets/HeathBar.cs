using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LifeStats))]
public class HeathBar : MonoBehaviour {

    [SerializeField]
    private float healthMax = 100f;
    [SerializeField]
    private float healthCurrent = 100f;
    public GameObject healthCanvas;



    private LifeStats lifeStats;

	// Use this for initialization
	void Start () {

        lifeStats = GetComponent<LifeStats>();

        OnMyStatsUpdate(); //update current stats
        lifeStats.OnStats += OnMyStatsUpdate; // and subscribe for  health changes


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateHealth()
    {

        
        float h = ((healthCurrent ) / (healthMax) );
        if (h < 0 )
        {
            h = 0;
        }
        else if (h > healthMax)
        {
            h = healthMax;
        }

        //healthCanvas.transform.localScale = new Vector3(h, 1, 1);
        healthCanvas.transform.localScale = new Vector3(h, healthCanvas.transform.localScale.y, healthCanvas.transform.localScale.z);
    }

    public void OnMyStatsUpdate()
    {
        healthCurrent = lifeStats.healthCurrent;
        healthMax = lifeStats.healthMax;
        UpdateHealth();
    }
}

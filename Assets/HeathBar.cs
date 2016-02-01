using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LifeStats))]
public class HeathBar : MonoBehaviour {

    [SerializeField]
    private float healthMax = 100f;
    [SerializeField]
    private float healthCurrent = 100f;
    public GameObject healthBar;
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
        else if (h > 1)
        {
            h = 1;
        }
        else if (h > 0.98)
        {
            CanvasHide();
        }
        else
        {
            CanvasShow();
        }

        //healthCanvas.transform.localScale = new Vector3(h, 1, 1);
        healthBar.transform.localScale = new Vector3(h, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

    }

    public void OnMyStatsUpdate()
    {
        healthCurrent = lifeStats.healthCurrent;
        healthMax = lifeStats.healthMax;
        UpdateHealth();
    }

    void CanvasHide()
    {
        if (healthCanvas.active) healthCanvas.active = false;
    }

    void CanvasShow()
    {
        if (!healthCanvas.active) healthCanvas.active = true;
    }
}

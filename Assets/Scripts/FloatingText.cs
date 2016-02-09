using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour {

    public GameObject textPrefab;
    //private Text text;
    //private GUI
    public int lifeTime = 2;
    
    //private Vector3 locScale;
    //private Vector3 locPosition;

    private Vector3 flyDirection = new Vector3(1, 1, 1);
    public float flySpeed = 0.1f;

    // Use this for initialization
    void Start () {

        Init();
        Destroy(this.gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePos();
    }

    void Init()
    {

        //GameObject textObj = Instantiate(textPrefab);
        //textObj.transform.parent = this.transform;

        
            
        //text = textObj.GetComponent<Text>();
        //locScale = text.te
        //locPosition = text.transform.position;
    }

    public void SetFloatingText(string message, Color color)
    {
        Text text;
        text = this.gameObject.GetComponentInChildren<Text>();
        text.text = message;
        text.color = color;
    }

    void  UpdatePos()
    {
        //this.transform.Translate(flyDirection * flySpeed * Time.deltaTime);
        this.transform.position += flyDirection * flySpeed * Time.deltaTime;
    }


}


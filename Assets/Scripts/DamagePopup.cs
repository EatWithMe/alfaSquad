using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour {


    public GameObject popupPrefub;

    public void GeneratePopup(string message, Color color)
    {
        if ( popupPrefub != null )
        {
            GameObject pop;
            pop = Instantiate( popupPrefub, this.transform.position , this.transform.rotation) as GameObject;
            FloatingText flText;
            flText = pop.GetComponent<FloatingText>();
            if ( flText!=null )
            {
                flText.SetFloatingText(message, color );
            }

        }

    }

/*
    public Color color =  new Color( 0.8f ,0.8f ,0f ,1.0f );
    public float scroll = 0.05f;  // scrolling velocity
    public float duration = 1.5f; // time to die
    public float alpha;
 
    void Start()
    {
        GetComponent<GUIText>().material.color = color; // set text color
        alpha = 1;
    }

    void Update()
    {
        if (alpha > 0)
        {
            Vector3 tV3 = transform.position;
            tV3.y += scroll * Time.deltaTime;
            transform.position = tV3;
            alpha -= Time.deltaTime / duration;
            //GUIText tmpText = GetComponent<GUIText>();
            Color colr = GetComponent<GUIText>().material.color;
            colr.a = alpha;
        }
        else {
            Destroy(gameObject); // text vanished - destroy itself
        }
    }




    */
}

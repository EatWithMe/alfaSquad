using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour {


    public Color color =  Color(0.8,0.8,0,1.0);
    public float scroll = 0.05;  // scrolling velocity
    public float duration = 1.5; // time to die
    public float alpha;
 
    void Start()
    {
        guiText.material.color = color; // set text color
        alpha = 1;
    }

    void Update()
    {
        if (alpha > 0)
        {
            transform.position.y += scroll * Time.deltaTime;
            alpha -= Time.deltaTime / duration;
            guiText.material.color.a = alpha;
        }
        else {
            Destroy(gameObject); // text vanished - destroy itself
        }
    }





}

using UnityEngine;
using System.Collections;

public class MoveToMouseClick : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetMouseButtonDown(1) )
        {
            Vector3 clickLocation;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log("you miss a surface with right click");
                if (hit.transform.tag == "Surface")
                {
                    clickLocation = hit.point;
                    AddMovingForce(clickLocation);
                }
                else
                {
                    Debug.Log("you miss a surface with right click");
                }
                
            }
            else
            {
                return;
            }
        }
	}

    void AddMovingForce(Vector3 wantedPos)
    {
        Vector3 relativePos = wantedPos - transform.position;

        // Move your game object using a rigid body force to get it moving in the right direction. 
        this.GetComponent<Rigidbody2D>().AddForce(relativePos.normalized * 40f);
    }
}

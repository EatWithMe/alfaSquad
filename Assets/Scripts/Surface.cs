using UnityEngine;
using System.Collections;

public class Surface : MonoBehaviour {

	

    public static bool GetMouseHitPosition(out Vector3 hitPos)
    {
        bool res = false;

        hitPos = new Vector3(0, 0, 0);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if ( hit.transform.tag == "Surface") 
            {
                hitPos = hit.point;
                res = true;
            }
        }
        else
        {
            
            //res = false;
        }

        return res;
    }

}

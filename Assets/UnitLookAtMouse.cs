using UnityEngine;
using System.Collections;

public class UnitLookAtMouse : MonoBehaviour {

    public float rotationSpeedMax = 200;//angels per sec

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        RotateUnitToMouse();
    }

    void RotateUnitToMouse()
    {
        Vector3 RotateToTarget;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            RotateToTarget = hit.point;
            //Debug.Log(RotateToTarget);
        }
        else
        {
            return;
        }



        Vector3 diff = RotateToTarget - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), rotationSpeedMax * Time.deltaTime);

    }
}



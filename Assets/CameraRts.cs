using UnityEngine;
using System.Collections;


public class CameraRts : MonoBehaviour//NetworkBehaviour
{

    public float speed = 20.0f; //cam speed
    public GameObject surface; // for map limits
    public GameObject middlePoint; //camera will not go fether then x
    public int maxDistance = 100;


    public int canFlyHeight = 9;

    int boundary = 1;
    
    int width;
    int height;

    int camMinX;
    int camMaxX;
    int camMinZ;
    int camMaxZ;

    void Start()
    {
        width = Screen.width;
        height = Screen.height;

        //we need to find Surface
        surface = GameObject.FindGameObjectWithTag("Surface");
        if (surface != null)
        {
            InitCameraPosition(surface);


            camMinX = (int)(surface.transform.position.x - surface.transform.localScale.x / 2);
            camMaxX = (int)(surface.transform.position.x + surface.transform.localScale.x / 2);
            camMinZ = (int)(surface.transform.position.z - surface.transform.localScale.z / 2);
            camMaxZ = (int)(surface.transform.position.z + surface.transform.localScale.z / 2);
        }
        else
        {
            Debug.Log("Cannot find game surface");
        }
        //    Mathf.Clamp(-100, 100);
    }

    void Update()
    {
        //if (isLocalPlayer)
        {
            MoveCamera();
        }
    }

    /// <summary>
    /// We need to put camera for looking at surface
    /// </summary>
    /// <param name="surf"></param>
    void InitCameraPosition(GameObject surf)
    {
        if (surf != null)
        {
            this.transform.position = surf.transform.position + new Vector3(0, canFlyHeight , 0);
            this.transform.LookAt(surf.transform);
        }
        else
        {
            Debug.Log("Cannot InitCameraPosition");
        }
    }



    void MoveCamera()
    {

        if (Input.mousePosition.x > width - boundary)
        {
            if (transform.position.x < camMaxX)
            {
                transform.position += new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);
            }
        }
        else if (Input.mousePosition.x < 0 + boundary)
        {
            if (transform.position.x > camMinX)
                transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);
        }

        if (Input.mousePosition.y > height - boundary)
        {
            if (transform.position.z < camMaxZ)
                transform.position += new Vector3(0.0f, 0.0f, Time.deltaTime * speed);
        }
        else if (Input.mousePosition.y < 0 + boundary)
        {
            if (transform.position.z > camMinZ)
                transform.position -= new Vector3(0.0f, 0.0f, Time.deltaTime * speed);
        }

        
        float distanceFromMidUnit = 0f;
        if (middlePoint)
        {
            //distanceFromMidUnit = Vector3.Distance(this.transform.position, middlePoint.transform.position);
            //if (distanceFromMidUnit > maxDistance)
            {
                Vector2 mid = new Vector2(middlePoint.transform.position.x, middlePoint.transform.position.z);
                Vector2 ths = new Vector2(this.transform.position.x, this.transform.position.z);

                distanceFromMidUnit = Vector2.Distance(mid, ths);
                if (distanceFromMidUnit > maxDistance)
                {
                    Vector2 diff = Vector2.ClampMagnitude(ths - mid, maxDistance);
                    //Debug.Log("vector = " + diff);

                    Vector3 corr = new Vector3(diff.x, 0f, diff.y);
                    Vector3 newPOs = middlePoint.transform.position + corr;

                    transform.position = new Vector3(newPOs.x, this.transform.position.y, newPOs.z);
                }
            }
        }
        
    }

}


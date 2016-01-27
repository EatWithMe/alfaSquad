using UnityEngine;
using System.Collections;


public class CameraRts : MonoBehaviour//NetworkBehaviour
{

    public float speed = 20.0f; //cam speed
    public GameObject surface; // for map limits

    int canFlyHeight = 9;
    int boundary = 1;

    int width;
    int height;

    int camMinX;
    int camMaxX;
    int camMinY;
    int camMaxY;

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
            camMinY = (int)(surface.transform.position.y - surface.transform.localScale.y / 2);
            camMaxY = (int)(surface.transform.position.y + surface.transform.localScale.y / 2);
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
            this.transform.position = surf.transform.position + new Vector3(0, 0, -1 * canFlyHeight);
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

        if (Input.mousePosition.x < 0 + boundary)
        {
            if (transform.position.x > camMinX)
                transform.position -= new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);
        }

        if (Input.mousePosition.y > height - boundary)
        {
            if (transform.position.y < camMaxY)
                transform.position += new Vector3(0.0f, Time.deltaTime * speed , 0.0f);
        }

        if (Input.mousePosition.y < 0 + boundary)
        {
            if (transform.position.y > camMinY)
                transform.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
        }
    }
}


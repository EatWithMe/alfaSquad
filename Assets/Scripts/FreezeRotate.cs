using UnityEngine;
using System.Collections;

public class FreezeRotate : MonoBehaviour {

    Quaternion rotation;
    Vector3 position;
    void Awake()
    {
        rotation = transform.rotation;
        position = transform.localPosition;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
        transform.position = transform.parent.position  + position;
    }
}

using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

    public int moveSpeed = 20;
    public int lifeTimeSec = 5;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        Destroy(gameObject, lifeTimeSec);
	}
}

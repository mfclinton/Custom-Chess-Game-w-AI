using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotatingScript : MonoBehaviour {

    public Transform rotateArroundObject;
	// Update is called once per frame
	void Update () {
        transform.LookAt(rotateArroundObject);
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}

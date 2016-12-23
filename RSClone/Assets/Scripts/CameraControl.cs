using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private float CameraHeight;
    private float CameraHeightDiff = 0.0f;
    public float maxCameraHeightDiff = 5.0f;
    public float rotateSpeed = 5.0f;
    private float angle = Mathf.PI;
	// Use this for initialization
	void Start () {
        CameraHeight = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
        float dir = Input.GetAxis("Horizontal");
        angle -= dir * rotateSpeed * Time.deltaTime;

        dir = Input.GetAxis("Vertical");
        CameraHeightDiff += Time.deltaTime * dir * maxCameraHeightDiff;
        if (CameraHeightDiff < -maxCameraHeightDiff)
            CameraHeightDiff = -maxCameraHeightDiff;
        if (CameraHeightDiff > maxCameraHeightDiff)
            CameraHeightDiff = maxCameraHeightDiff;

        transform.localPosition = new Vector3(Mathf.Sin(angle) * 10, CameraHeight + CameraHeightDiff, Mathf.Cos(angle) * 10);

        transform.LookAt(transform.parent);
	}
}

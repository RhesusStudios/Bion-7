using UnityEngine;
using System.Collections;

public class lookAt : MonoBehaviour {


	GameObject camCam;

	// Use this for initialization
	void Start () {

		camCam = GameObject.Find ("Main Camera");
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.LookAt (camCam.transform);
	
	}
}

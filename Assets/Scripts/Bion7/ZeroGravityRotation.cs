using UnityEngine;
using System.Collections;

public class ZeroGravityRotation : MonoBehaviour {

	[Range (0f, 1f)]
	public float rotationVelocity = 0.5f;
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(rotationVelocity, rotationVelocity, rotationVelocity);
	}
}

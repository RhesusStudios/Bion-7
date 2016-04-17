using UnityEngine;
using System.Collections;

public class ZeroGravityDirectedFloat : MonoBehaviour
{
	[Range (0f, 10f)]
	public float velocity = 0.5f;

	private Rigidbody _rigidbody;

	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_rigidbody.AddForce(Vector3.up * velocity);
	}
}

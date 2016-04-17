using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour
{
	public bool isControllable = true;

	// Use this for initialization
	void Start ()
	{	
		// or you can use the SensorHelper, which has built-in fallback to less accurate but more common sensors:
		SensorHelper.ActivateRotation();
	}

	// Update is called once per frame
	void Update ()
	{
		if  (isControllable)
		{
			// Helper with fallback:
			transform.rotation = SensorHelper.rotation;
			Vector3 rot = transform.rotation.eulerAngles;
			transform.eulerAngles = new Vector3(0f, 0f, rot.z);
		}
	}
}

using UnityEngine;
using System.Collections;
using LostPolygon.AndroidBluetoothMultiplayer.Examples;
using LostPolygon.AndroidBluetoothMultiplayer;

public class GyroController : MonoBehaviour
{
	public float Speed = 100f;
	public double NetworkInterpolationBackTime = 0.11;

	private Vector3 _destination;
	private Transform _transform;
	private Renderer _renderer;
	private NetworkView _networkView;
	private NetworkTransformInterpolation _transformInterpolation;
	private string _actorTag = "None";
	private PlayerManager _playerManager;

	private readonly Color[] kColors = {
		Color.blue,
		Color.cyan,
		Color.green,
		Color.magenta,
		Color.red,
		Color.white,
		Color.yellow
	};

	private void Awake ()
	{
		_transform = GetComponent<Transform>();
		_renderer = GetComponent<Renderer>();
		_networkView = GetComponent<NetworkView>();
		_playerManager = GetComponent<PlayerManager>();
		_destination = transform.position;
//		_renderer.material.color = kColors[Random.Range(0, kColors.Length)];

		_transformInterpolation = new NetworkTransformInterpolation();
		_transformInterpolation.InterpolationBackTime = NetworkInterpolationBackTime;
	}

	// Use this for initialization
	void Start ()
	{	
		// or you can use the SensorHelper, which has built-in fallback to less accurate but more common sensors:
		SensorHelper.ActivateRotation();
		
		useGUILayout = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if  (isMine())
		{
			// Helper with fallback:
			transform.rotation = SensorHelper.rotation;
			Vector3 rot = transform.rotation.eulerAngles;
			transform.eulerAngles = new Vector3(0f, 0f, rot.z);
		}

//		if (_networkView.isMine) {
//			_destination.z = 0f;
//			Vector3 direction = _destination - transform.position;
//			
//			if (direction.magnitude > 1f)
//				transform.Translate(Speed * direction.normalized * Time.deltaTime);
//			
//			if (Input.GetMouseButtonDown(0)) {
//				_destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			}
//		} else {
//			Vector3 interpolatedPosition = _transform.position;
//			Quaternion interpolatedRotation = _transform.rotation;
//			_transformInterpolation.Update(ref interpolatedPosition, ref interpolatedRotation);
//			
//			_transform.position = interpolatedPosition;
//			_transform.rotation = interpolatedRotation;
//		}
	}

	private void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		// Serialize the position and color
		Vector3 pos = new Vector3();
		Vector3 rot = new Vector3();

		if (stream.isWriting)
		{
			if (isMine() == true)
			{
//				Color color = _renderer.material.color;
//				NetworkUtilities.Serialize (ref stream, ref color);

				pos = transform.position;
				rot = transform.eulerAngles;

				NetworkUtilities.Serialize (ref stream, ref pos);
				NetworkUtilities.Serialize (ref stream, ref rot);
			}
		}
		else
		{
			if (isMine() == false)
			{
//				Color color = Color.white;
//				NetworkUtilities.Serialize (ref stream, ref color);
				NetworkUtilities.Serialize (ref stream, ref pos);
				NetworkUtilities.Serialize (ref stream, ref rot);

				transform.position = pos;
				transform.eulerAngles = rot;
				
//				_renderer.material.color = color;
			}
		}
		
//		_transformInterpolation.OnSerializeNetworkView(stream, info, _transform.position, _transform.rotation);
	}

	public bool isMine ()
	{
		if (Network.isServer && _playerManager.playerNumber == PlayerNumber.PlayerOne)
			return true;
		else if (Network.isClient && _playerManager.playerNumber == PlayerNumber.PlayerTwo)
			return true;
		else
			return false;
	}
}

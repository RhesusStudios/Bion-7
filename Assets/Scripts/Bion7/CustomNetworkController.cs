using UnityEngine;
using System.Collections;

public class CustomNetworkController : MonoBehaviour
{
	private PlayerManager _playerManager;
	private SpaceShipControl _spaceShipController;

	// Use this for initialization
	void Start ()
	{
		_playerManager = GetComponent<PlayerManager>();
		_spaceShipController = GetComponent<SpaceShipControl>();

		if (isMine())
		{
			//MINE: local player, simply enable the local scripts
			_spaceShipController.enabled = true;
		}
		else
		{	
			_spaceShipController.enabled = true;
			_spaceShipController.isControllable = false;
		}
	}
	
	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	
	void Update()
	{
		if (!isMine())
		{
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
		}
	}

	private void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		// Serialize the position and color
		Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;
		
		if (stream.isWriting)
		{
			if (isMine() == true)
			{
//				Color color = _renderer.material.color;
//				NetworkUtilities.Serialize (ref stream, ref color);
				
				pos = transform.position;
				rot = transform.rotation;
				
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
				
				correctPlayerPos = pos;
				correctPlayerRot = rot;
				
//				_renderer.material.color = color;
			}
		}
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

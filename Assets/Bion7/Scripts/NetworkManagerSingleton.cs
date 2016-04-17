using UnityEngine;
using System.Collections;
using LostPolygon.AndroidBluetoothMultiplayer;

public class NetworkManagerSingleton : MonoBehaviour
{
	private static NetworkManagerSingleton _instance;
	public static NetworkManagerSingleton Instance
	{
		get
		{
//			if (_instance == null)
//				_instance = new NetworkManagerSingleton();
			return _instance;
		}
	}

	public void Awake ()
	{
		_instance = this;
	}

	public BluetoothMultiplayerMode DesiredMode = BluetoothMultiplayerMode.None;
	public int PlayerCount = 0;
}

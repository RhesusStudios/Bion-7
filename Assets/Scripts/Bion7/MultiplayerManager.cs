using UnityEngine;
using System.Collections;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples
{
	public class MultiplayerManager : BluetoothDemoGuiBase {
//		public GameObject LeftActorPrefab; // Reference to the player actor
//		public GameObject RightActorPrefab; // Reference to the enemy actor
//
//		public Transform SpawnPointSelf;
//		public Transform SpawnPointParty;

		public GameObject CreateServerButton;
		public GameObject ConnectToServerButton;
		public GameObject DisconnectOrStopServerButton;
		public GameObject WaitingForOpponentToJoinLabel;
		public GameObject ReadyToLoadGameScene;

		private NetworkView _networkView;
		private bool _readyToLoadGameScene = false;
		
		#if !UNITY_ANDROID
		private void Awake() {
			Debug.LogError("Build platform is not set to Android. Please choose Android as build Platform in File - Build Settings...");
		}
		
		private void OnGUI() {
			GUI.Label(new Rect(10, 10, Screen.width, 100), "Build platform is not set to Android. Please choose Android as build Platform in File - Build Settings...");
		}
		#else
		private const string kLocalIp = "127.0.0.1"; // An IP for Network.Connect(), must always be 127.0.0.1
		private const int kPort = 28000; // Local server IP. Must be the same for client and server
		
		private bool _initResult;
		private BluetoothMultiplayerMode _desiredMode = BluetoothMultiplayerMode.None;
		
		private void Awake() {
			// Setting the UUID. Must be unique for every application
			_initResult = AndroidBluetoothMultiplayer.Initialize("8ce255c0-200a-11e0-ac64-0800200c9a66");
			
			// Enabling verbose logging. See log cat!
			AndroidBluetoothMultiplayer.SetVerboseLog(true);
			
			// Registering the event delegates
			AndroidBluetoothMultiplayer.ListeningStarted += OnBluetoothListeningStarted;
			AndroidBluetoothMultiplayer.ListeningStopped += OnBluetoothListeningStopped;
			AndroidBluetoothMultiplayer.AdapterEnabled += OnBluetoothAdapterEnabled;
			AndroidBluetoothMultiplayer.AdapterEnableFailed += OnBluetoothAdapterEnableFailed;
			AndroidBluetoothMultiplayer.AdapterDisabled += OnBluetoothAdapterDisabled;
			AndroidBluetoothMultiplayer.DiscoverabilityEnabled += OnBluetoothDiscoverabilityEnabled;
			AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed += OnBluetoothDiscoverabilityEnableFailed;
			AndroidBluetoothMultiplayer.ConnectedToServer += OnBluetoothConnectedToServer;
			AndroidBluetoothMultiplayer.ConnectionToServerFailed += OnBluetoothConnectionToServerFailed;
			AndroidBluetoothMultiplayer.DisconnectedFromServer += OnBluetoothDisconnectedFromServer;
			AndroidBluetoothMultiplayer.ClientConnected += OnBluetoothClientConnected;
			AndroidBluetoothMultiplayer.ClientDisconnected += OnBluetoothClientDisconnected;
			AndroidBluetoothMultiplayer.DevicePicked += OnBluetoothDevicePicked;

			_networkView = GetComponent<NetworkView>();
		}
		
		// Don't forget to unregister the event delegates!
		protected override void OnDestroy() {
			base.OnDestroy();
			
			AndroidBluetoothMultiplayer.ListeningStarted -= OnBluetoothListeningStarted;
			AndroidBluetoothMultiplayer.ListeningStopped -= OnBluetoothListeningStopped;
			AndroidBluetoothMultiplayer.AdapterEnabled -= OnBluetoothAdapterEnabled;
			AndroidBluetoothMultiplayer.AdapterEnableFailed -= OnBluetoothAdapterEnableFailed;
			AndroidBluetoothMultiplayer.AdapterDisabled -= OnBluetoothAdapterDisabled;
			AndroidBluetoothMultiplayer.DiscoverabilityEnabled -= OnBluetoothDiscoverabilityEnabled;
			AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed -= OnBluetoothDiscoverabilityEnableFailed;
			AndroidBluetoothMultiplayer.ConnectedToServer -= OnBluetoothConnectedToServer;
			AndroidBluetoothMultiplayer.ConnectionToServerFailed -= OnBluetoothConnectionToServerFailed;
			AndroidBluetoothMultiplayer.DisconnectedFromServer -= OnBluetoothDisconnectedFromServer;
			AndroidBluetoothMultiplayer.ClientConnected -= OnBluetoothClientConnected;
			AndroidBluetoothMultiplayer.ClientDisconnected -= OnBluetoothClientDisconnected;
			AndroidBluetoothMultiplayer.DevicePicked -= OnBluetoothDevicePicked;
		}

		private void Update ()
		{
			float scaleFactor = BluetoothExamplesTools.UpdateScaleMobile();
			// If initialization was successfull, showing the buttons
			if (_initResult)
			{
				// If there is no current Bluetooth connectivity
				BluetoothMultiplayerMode currentMode = AndroidBluetoothMultiplayer.GetCurrentMode();

				if (_readyToLoadGameScene)
				{
					CreateServerButton.SetActive(false);
					ConnectToServerButton.SetActive(false);
					DisconnectOrStopServerButton.SetActive(false);
					WaitingForOpponentToJoinLabel.SetActive(false);
					ReadyToLoadGameScene.SetActive(true);
				}
				else
				{
					if (currentMode == BluetoothMultiplayerMode.None)
					{
						CreateServerButton.SetActive(true);
						ConnectToServerButton.SetActive(true);
						DisconnectOrStopServerButton.SetActive(false);
						WaitingForOpponentToJoinLabel.SetActive(false);
						ReadyToLoadGameScene.SetActive(false);
					}
					else
					{
						CreateServerButton.SetActive(false);
						ConnectToServerButton.SetActive(false);
						DisconnectOrStopServerButton.SetActive(true);
						ReadyToLoadGameScene.SetActive(false);

						if (currentMode == BluetoothMultiplayerMode.Server)
						{
							WaitingForOpponentToJoinLabel.SetActive(true);
						}
						else
						{
							WaitingForOpponentToJoinLabel.SetActive(false);
						}
					}
				}
			}
		}
		
//		private void OnGUI() {
//			float scaleFactor = BluetoothExamplesTools.UpdateScaleMobile();
//			// If initialization was successfull, showing the buttons
//			if (_initResult) {
//				// If there is no current Bluetooth connectivity
//				BluetoothMultiplayerMode currentMode = AndroidBluetoothMultiplayer.GetCurrentMode();
//				if (currentMode == BluetoothMultiplayerMode.None) {
//					if (GUI.Button(new Rect(10, 10, 150, 50), "Create server")) {
//						// If Bluetooth is enabled, then we can do something right on
//						if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
//							AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
//							Network.Disconnect(); // Just to be sure
//							AndroidBluetoothMultiplayer.StartServer(kPort);
//						} else {
//							// Otherwise we have to enable Bluetooth first and wait for callback
//							_desiredMode = BluetoothMultiplayerMode.Server;
//							AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
//						}
//					}
//					
//					if (GUI.Button(new Rect(170, 10, 150, 50), "Connect to server")) {
//						// If Bluetooth is enabled, then we can do something right on
//						if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
//							Network.Disconnect(); // Just to be sure
//							AndroidBluetoothMultiplayer.ShowDeviceList(); // Open device picker dialog
//						} else {
//							// Otherwise we have to enable Bluetooth first and wait for callback
//							_desiredMode = BluetoothMultiplayerMode.Client;
//							AndroidBluetoothMultiplayer.RequestEnableBluetooth();
//						}
//					}
//				} else {
//					// Stop all networking
//					if (GUI.Button(new Rect(10, 10, 150, 50), currentMode  == BluetoothMultiplayerMode.Client ? "Disconnect" : "Stop server")) {
//						if (Network.peerType != NetworkPeerType.Disconnected)
//							Network.Disconnect();
//					}
//				}
//			} else {
//				// Show a message if initialization failed for some reason
//				GUI.contentColor = Color.black;
//				GUI.Label(
//					new Rect(10, 10, Screen.width / scaleFactor - 10, 50), 
//					"Bluetooth not available. Are you running this on Bluetooth-capable " +
//					"Android device and AndroidManifest.xml is set up correctly?");
//			}
//		}

		public void CreateServer ()
		{
			// If Bluetooth is enabled, then we can do something right on
			if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled())
			{
				AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
				Network.Disconnect(); // Just to be sure
				AndroidBluetoothMultiplayer.StartServer(kPort);
			}
			else
			{
				// Otherwise we have to enable Bluetooth first and wait for callback
				_desiredMode = BluetoothMultiplayerMode.Server;
				AndroidBluetoothMultiplayer.RequestEnableDiscoverability(120);
			}
		}

		public void ConnectToServer ()
		{
			// If Bluetooth is enabled, then we can do something right on
			if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled())
			{
				Network.Disconnect(); // Just to be sure
				AndroidBluetoothMultiplayer.ShowDeviceList(); // Open device picker dialog
			}
			else
			{
				// Otherwise we have to enable Bluetooth first and wait for callback
				_desiredMode = BluetoothMultiplayerMode.Client;
				AndroidBluetoothMultiplayer.RequestEnableBluetooth();
			}
		}

		public void DisconnectOrStopServer ()
		{
			if (Network.peerType != NetworkPeerType.Disconnected)
				Network.Disconnect();
		}
		
		protected override void OnBackToMenu() {
			// Gracefully closing all Bluetooth connectivity and loading the menu
			try {
				AndroidBluetoothMultiplayer.StopDiscovery();
				AndroidBluetoothMultiplayer.Stop();
			} catch {
				// 
			}
		}
		
		#region Bluetooth events
		
		private void OnBluetoothListeningStarted() {
			Debug.Log("Event - ListeningStarted");
			
			// Starting Unity networking server if Bluetooth listening started successfully
			Network.InitializeServer(4, kPort, false);
		}
		
		private void OnBluetoothListeningStopped() {
			Debug.Log("Event - ListeningStopped");
			
			// For demo simplicity, stop server if listening was canceled
			AndroidBluetoothMultiplayer.Stop();
		}
		
		private void OnBluetoothDevicePicked(BluetoothDevice device) {
			Debug.Log("Event - DevicePicked: " + BluetoothExamplesTools.FormatDevice(device));
			
			// Trying to connect to a device user had picked
			AndroidBluetoothMultiplayer.Connect(device.Address, kPort);
		}
		
		private void OnBluetoothClientDisconnected(BluetoothDevice device) {
			Debug.Log("Event - ClientDisconnected: " + BluetoothExamplesTools.FormatDevice(device));
		}
		
		private void OnBluetoothClientConnected(BluetoothDevice device) {
			Debug.Log("Event - ClientConnected: " + BluetoothExamplesTools.FormatDevice(device));
		}
		
		private void OnBluetoothDisconnectedFromServer(BluetoothDevice device) {
			Debug.Log("Event - DisconnectedFromServer: " + BluetoothExamplesTools.FormatDevice(device));
			
			// Stopping Unity networking on Bluetooth failure
			Network.Disconnect();
		}
		
		private void OnBluetoothConnectionToServerFailed(BluetoothDevice device) {
			Debug.Log("Event - ConnectionToServerFailed: " + BluetoothExamplesTools.FormatDevice(device));
		}
		
		private void OnBluetoothConnectedToServer(BluetoothDevice device) {
			Debug.Log("Event - ConnectedToServer: " + BluetoothExamplesTools.FormatDevice(device));
			
			// Trying to negotiate a Unity networking connection, 
			// when Bluetooth client connected successfully
			Network.Connect(kLocalIp, kPort);
		}
		
		private void OnBluetoothAdapterDisabled() {
			Debug.Log("Event - AdapterDisabled");
		}
		
		private void OnBluetoothAdapterEnableFailed() {
			Debug.Log("Event - AdapterEnableFailed");
		}
		
		private void OnBluetoothAdapterEnabled() {
			Debug.Log("Event - AdapterEnabled");
			
			// Resuming desired action after enabling the adapter
			switch (_desiredMode) {
			case BluetoothMultiplayerMode.Server:
				Network.Disconnect();
				AndroidBluetoothMultiplayer.StartServer(kPort);
				break;
			case BluetoothMultiplayerMode.Client:
				Network.Disconnect();
				AndroidBluetoothMultiplayer.ShowDeviceList();
				break;
			}
			
			_desiredMode = BluetoothMultiplayerMode.None;
		}
		
		private void OnBluetoothDiscoverabilityEnableFailed() {
			Debug.Log("Event - DiscoverabilityEnableFailed");
		}
		
		private void OnBluetoothDiscoverabilityEnabled(int discoverabilityDuration) {
			Debug.Log(string.Format("Event - DiscoverabilityEnabled: {0} seconds", discoverabilityDuration));
		}
		
		#endregion Bluetooth events
		
		#region Network events
		
		private void OnPlayerDisconnected(NetworkPlayer player) {
			Debug.Log("Player disconnected: " + player.GetHashCode());
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
			NetworkManagerSingleton.Instance.PlayerCount--;
		}
		
		private void OnFailedToConnect(NetworkConnectionError error) {
			Debug.Log("Can't connect to the networking server");
			
			// Stopping all Bluetooth connectivity on Unity networking disconnect event
			AndroidBluetoothMultiplayer.Stop();
		}
		
		private void OnDisconnectedFromServer() {
			Debug.Log("Disconnected from server");
			
			// Stopping all Bluetooth connectivity on Unity networking disconnect event
			AndroidBluetoothMultiplayer.Stop();
			
			GyroController[] objects = FindObjectsOfType(typeof(GyroController)) as GyroController[];
			if (objects != null) {
				foreach (GyroController obj in objects) {
					Destroy(obj.gameObject);
				}
			}
		}
		
		private void OnConnectedToServer() {
			Debug.Log("Connected to server");
			NetworkManagerSingleton.Instance.PlayerCount++;
//			LoadGameScene();
			_networkView.RPC ("ClientDidJoin", RPCMode.Server);
			
			// Instantiating a simple test actor
//			Network.Instantiate(RightActorPrefab, new Vector3(3f, 0f, 0f), Quaternion.identity, 0);

//			LeftActorPrefab.transform.tag = "Enemy";
//			RightActorPrefab.transform.tag = "Player";
		}
		
		private void OnServerInitialized() {
			Debug.Log("Server initialized");
			NetworkManagerSingleton.Instance.PlayerCount++;
//			LoadGameScene();
			
			// Instantiating a simple test actor
			if (Network.isServer) {
//				Network.Instantiate(LeftActorPrefab, new Vector3(-3f, 0f, 0f), Quaternion.identity, 0);

//				LeftActorPrefab.transform.tag = "Player";
//				RightActorPrefab.transform.tag = "Enemy";
			}
		}

		IEnumerator LoadGameScene ()
		{
			_readyToLoadGameScene = true;
			yield return new WaitForSeconds(0.5f);
			Application.LoadLevel(1);
		}

		[RPC]
		public void ClientDidJoin ()
		{
			_networkView.RPC ("LoadGameSceneTogether", RPCMode.All);
		}

		[RPC]
		public void LoadGameSceneTogether ()
		{
			StartCoroutine(LoadGameScene());
		}
		
		#endregion Network events
		#endif
	}
}
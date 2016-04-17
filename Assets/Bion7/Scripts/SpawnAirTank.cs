using UnityEngine;
using System.Collections;

public class SpawnAirTank : MonoBehaviour
{
	public GameObject airTankPrefab;

	[Range (5, 20)]
	public float spawnMaxIntervalInSeconds = 10;

	private NetworkView _networkView;
	private GameObject _spawnedAirTank;

	// Use this for initialization
	void Start ()
	{
		_networkView = GetComponent<NetworkView>();
	}

	IEnumerator SpawnAirTanksPeriodicaly ()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5, spawnMaxIntervalInSeconds));
			if (_spawnedAirTank)
			{
				_networkView.RPC ("DestroyAirTank", RPCMode.All);
				yield return new WaitForEndOfFrame();
			}
			_networkView.RPC ("InstantiateAirTank", RPCMode.All);
		}
	}

	[RPC]
	public void InstantiateAirTank ()
	{
		_spawnedAirTank = Instantiate(airTankPrefab, transform.position, transform.rotation) as GameObject;
	}

	[RPC]
	public void DestroyAirTank ()
	{
		Destroy(_spawnedAirTank);
	}
}

using UnityEngine;
using System.Collections;

public class GamePlayManager : MonoBehaviour
{
	private bool _isGameStarted = false;
	private bool _isGameFinished = false;
	private PlayerNumber _winner = PlayerNumber.PlayerOne;

	// Use this for initialization
	void Start ()
	{
		_isGameStarted = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

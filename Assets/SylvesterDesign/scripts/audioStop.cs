using UnityEngine;
using System.Collections;

public class audioStop : MonoBehaviour {


	AudioSource aud;
	public string LevelToLoad;

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!aud.isPlaying) {
			Application.LoadLevel(LevelToLoad);
		}
	
	}
}

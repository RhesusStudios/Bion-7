using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthFiller : MonoBehaviour {

	public Image Air;
	float airFill = 1;

	float timer;
	float delay;

	// Use this for initialization
	void Start () {
		timer = Time.time;
		delay = Random.Range (0.5f, 2f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if ((Time.time - timer) > delay) {
			timer = Time.time;
			delay = Random.Range (0.5f, 2f);
			airFill -= 0.03f;
			Air.fillAmount = airFill;
		}
	}

	public void Boost(){
		airFill -= 0.1f;
		Air.fillAmount = airFill;
	}

	public void addAir(){
		airFill += 0.15f;
		Air.fillAmount = airFill;
	}
}

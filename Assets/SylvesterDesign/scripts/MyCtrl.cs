using UnityEngine;
using System.Collections;

public class MyCtrl : MonoBehaviour {

	GameObject character;

	Vector3 target;

	bool walking;

	public float speed = 5;

	public Vector3 HitPoint;
	GameObject hitsObject;
	Camera camCam;

	public GameObject player;
	Animator anim;

	// Use this for initialization
	void Start () {

		camCam = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();

		anim = player.GetComponent<Animator>();

		character = transform.gameObject;

		target = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			Ray rayOrigin = camCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;

			float distance = 100f;

			if (Physics.Raycast (rayOrigin, out hitInfo, distance, 1 << LayerMask.NameToLayer ("Invisible"))) {
				HitPoint = hitInfo.point;
				hitsObject = hitInfo.collider.gameObject;
				
			}

			target = new Vector3 (HitPoint.x, HitPoint.y, character.transform.position.z);
			walking = true;
		}

		if (walking) {
			anim.SetBool("03", true);
			transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);

			if(HitPoint.x > transform.position.x){
				player.transform.localScale = new Vector3 (1,1,1);
			}else{
				player.transform.localScale = new Vector3 (1,1,-1);
			}

		}

		if (transform.position == target) {
			anim.SetBool("03", false);
			walking = false;
		}
	
	}
}

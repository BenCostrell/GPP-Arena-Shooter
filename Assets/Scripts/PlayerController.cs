using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float forceFactor;
	public GameObject bulletPrefab;
	public float bulletOffsetFactor;
	public float timeBetweenFires;
	private float timeUntilNextShot;

	// Use this for initialization
	void Start () {
		timeUntilNextShot = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		FaceTheCursor ();

		if (timeUntilNextShot <= 0) {
			Shoot ();
			timeUntilNextShot = timeBetweenFires;
		} else {
			timeUntilNextShot -= Time.deltaTime;
		}
	}

	void Move(){
		Vector2 forceToAdd = Vector2.zero;
		if (Input.GetAxis("Horizontal") > 0){
			forceToAdd += Vector2.right;
		}
		if (Input.GetAxis ("Horizontal") < 0){
			forceToAdd += Vector2.left;
		}
		if (Input.GetAxis("Vertical") > 0){
			forceToAdd += Vector2.up;
		}
		if (Input.GetAxis ("Vertical") < 0){
			forceToAdd += Vector2.down;
		}
			
		GetComponent<Rigidbody2D> ().AddForce (forceFactor * forceToAdd.normalized);
	}

	void FaceTheCursor(){
		Vector3 distanceToMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float angleBetweenShipAndMouse = Mathf.Rad2Deg * Mathf.Atan2 (distanceToMouse.y, distanceToMouse.x);

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angleBetweenShipAndMouse - 90);
	}

	void Shoot(){
		float angleInDegrees = Mathf.Deg2Rad * (transform.eulerAngles.z + 90);
		Vector3 rotationVector = new Vector2 (Mathf.Cos (angleInDegrees), Mathf.Sin(angleInDegrees));

		Instantiate (bulletPrefab, transform.position + (rotationVector.normalized * bulletOffsetFactor), transform.rotation);
	}
}

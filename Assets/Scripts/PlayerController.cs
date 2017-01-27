using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float forceFactor;
	public GameObject bulletPrefab;
	public float bulletOffsetFactor;
	public float timeBetweenFires;
	private float timeSinceLastFire;

	// Use this for initialization
	void Start () {
		timeSinceLastFire = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		FaceTheCursor ();

		if (timeSinceLastFire > timeBetweenFires) {
			Shoot ();
			timeSinceLastFire = 0;
		} else {
			timeSinceLastFire += Time.deltaTime;
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
		Vector3 rotationVector = new Vector3 (Mathf.Tan (Mathf.Deg2Rad * transform.eulerAngles.z), 1, 0);

		Instantiate (bulletPrefab, transform.position + (rotationVector.normalized * bulletOffsetFactor), transform.rotation);
	}
}

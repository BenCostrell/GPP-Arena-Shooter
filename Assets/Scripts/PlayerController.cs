using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject bulletPrefab;
	public float bulletOffsetFactor;
	public float timeBetweenFires;
	private float timeUntilNextShot;
	public GameManager gameManager;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		timeUntilNextShot = 0;
		rb = GetComponent<Rigidbody2D> ();
		gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ();
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
		Vector2 direction = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		direction = direction.normalized;

		rb.velocity = speed * direction;
	}

	void FaceTheCursor(){
		Vector3 distanceToMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float angleBetweenShipAndMouse = Mathf.Rad2Deg * Mathf.Atan2 (distanceToMouse.y, distanceToMouse.x);

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angleBetweenShipAndMouse - 90);
	}

	void Shoot(){
		float angleInRadians = Mathf.Deg2Rad * (transform.eulerAngles.z + 90);
		Vector3 rotationVector = new Vector3 (Mathf.Cos (angleInRadians), Mathf.Sin(angleInRadians));

		Instantiate (bulletPrefab, transform.position + (rotationVector.normalized * bulletOffsetFactor), transform.rotation);
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject collidedObject = collider.gameObject;
		if (collidedObject.tag == "Enemy") {
			Die ();
		}
	}

	void Die(){
		Destroy (gameObject);
		gameManager.EndGame ();
	}
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float bulletOffsetFactor;
	public float timeBetweenFires;
	private float timeUntilNextShot;
	private AudioSource laserSound;
	private AudioSource deathSound;
	private bool inputEnabled;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		inputEnabled = true;
		timeUntilNextShot = 0;
		rb = GetComponent<Rigidbody2D> ();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		laserSound = audioSources [0];
		deathSound = audioSources [1];
	}
	
	// Update is called once per frame
	void Update () {
		if (inputEnabled) {
			Move ();
			FaceTheCursor ();

			if (timeUntilNextShot <= 0) {
				Shoot ();
				timeUntilNextShot = timeBetweenFires;
			} else {
				timeUntilNextShot -= Time.deltaTime;
			}
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

		Instantiate (Services.PrefabDB.bullet, transform.position + (rotationVector.normalized * bulletOffsetFactor), transform.rotation);

		laserSound.Play ();
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject collidedObject = collider.gameObject;
		if (collidedObject.tag == "Enemy") {
			Die ();
		}
	}

	void DisableInput(){
		inputEnabled = false;
	}

	void EnableInput(){
		inputEnabled = true;
	}

	void Die(){
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		DisableInput ();
		deathSound.Play ();
		float audioLength = deathSound.clip.length;
		Destroy (gameObject, audioLength);
		Services.GameManager.EndGame ();
	}
}

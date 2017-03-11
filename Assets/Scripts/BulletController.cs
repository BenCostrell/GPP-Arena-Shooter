using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speed;
	public float lifetime;

	// Use this for initialization
	void Start () {
		float angleInDegrees = Mathf.Deg2Rad * (transform.eulerAngles.z + 90);
		Vector3 rotationVector = new Vector2 (Mathf.Cos (angleInDegrees), Mathf.Sin(angleInDegrees));
		GetComponent<Rigidbody2D> ().velocity = speed * rotationVector.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<Renderer> ().isVisible) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject collidedObject = collider.gameObject;
		if (collidedObject.tag == "Enemy") {
			Destroy (gameObject);
			collidedObject.GetComponent<Enemy> ().TakeDamage (1);
		}
	}
}

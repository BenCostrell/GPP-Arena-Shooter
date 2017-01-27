using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speed;
	public float lifetime;

	// Use this for initialization
	void Start () {
		float angleInDegrees = Mathf.Deg2Rad * (transform.eulerAngles.z + 90);
		Vector3 rotationVector = new Vector2 (Mathf.Cos (angleInDegrees), Mathf.Sin(angleInDegrees));
		Debug.Log (rotationVector);
		GetComponent<Rigidbody2D> ().velocity = speed * rotationVector.normalized;
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

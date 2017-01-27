using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speed;
	public float lifetime;

	// Use this for initialization
	void Start () {
		Vector3 rotationVector = new Vector2 (Mathf.Tan (Mathf.Deg2Rad * transform.eulerAngles.z), 1);
		Debug.Log (rotationVector);
		GetComponent<Rigidbody2D> ().velocity = speed * rotationVector.normalized;
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

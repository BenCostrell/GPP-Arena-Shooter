using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float forceFactor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput ();
	}

	void ProcessInput(){
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

		GetComponent<Rigidbody2D> ().AddForce (forceFactor * forceToAdd);
	}
}

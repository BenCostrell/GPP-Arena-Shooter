using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	protected GameObject player;
	protected PlayerController playerCont;
	public float approachSpeed;
	public float avoidSpeed;
	public float coneFactor;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		coneFactor = 0.5f;
		player = GameObject.FindWithTag ("Player");
		playerCont = player.GetComponent<PlayerController> ();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	protected virtual void Move() {}

	protected void ApproachPlayer(){
		rb.MovePosition(Vector3.MoveTowards (transform.position, player.transform.position, approachSpeed * Time.deltaTime));
	}

	protected void AvoidPlayer(){
		rb.MovePosition (Vector3.MoveTowards (transform.position, player.transform.position, -avoidSpeed * Time.deltaTime));
	}

	protected bool IsPlayerFacingMe(){
		float angleInDegrees = Mathf.Deg2Rad * (player.transform.eulerAngles.z + 90);
		Vector3 playerRotationVector = new Vector3 (Mathf.Cos (angleInDegrees), Mathf.Sin (angleInDegrees)).normalized;
		Vector3 vectorFromPlayerToMe = (transform.position - player.transform.position).normalized;
		bool isFacing = Vector3.Dot (playerRotationVector, vectorFromPlayerToMe) > coneFactor;

		return isFacing;
	}

	public void Die(){
		Destroy (gameObject);
	}
}

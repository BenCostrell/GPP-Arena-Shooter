using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject player;
	protected PlayerController playerCont;
	protected float approachSpeed;
	protected float avoidSpeed;

	// Use this for initialization
	void Start () {
		playerCont = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	protected virtual void Move() {}

	protected void ApproachPlayer(){
		transform.Translate (Vector3.MoveTowards (transform.position, player.transform.position, approachSpeed));
	}

	protected void AvoidPlayer(){
		transform.Translate (Vector3.MoveTowards (transform.position, player.transform.position, -avoidSpeed));
	}
}

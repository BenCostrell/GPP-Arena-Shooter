using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	private GameObject player;
	private PlayerController playerCont;
	private GameManager gameManager;
	public float approachSpeed;
	public float avoidSpeed;
	public float zagSpeed;
	public float coneFactor;
	private Rigidbody2D rb;
	protected float zagTime;
	protected float timeUntilZag;


	// Use this for initialization
	void Start () {
		coneFactor = 0.5f;
		player = GameObject.FindWithTag ("Player");
		playerCont = player.GetComponent<PlayerController> ();
		rb = GetComponent<Rigidbody2D> ();
		gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ();
		InitializeSpeed ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			Move ();
		}
	}

	protected virtual void InitializeSpeed() {}

	protected virtual void Move() {}

	protected void ApproachPlayer(){
		rb.MovePosition(Vector3.MoveTowards (transform.position, player.transform.position, approachSpeed * Time.deltaTime));
	}

	protected void AvoidPlayer(){
		rb.MovePosition (Vector3.MoveTowards (transform.position, player.transform.position, -avoidSpeed * Time.deltaTime));
	}

	protected void Zag(){
		Vector3 vectorFromMeToPlayer = (player.transform.position - transform.position);
		float angle = Mathf.Rad2Deg * Mathf.Atan2 (vectorFromMeToPlayer.y, vectorFromMeToPlayer.x);
		float zagAngle = Mathf.Deg2Rad * (angle + 30);
		Vector3 zagVector = new Vector3 (Mathf.Cos (zagAngle), Mathf.Sin (zagAngle));
		rb.velocity = zagVector * zagSpeed;
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
		if (!gameManager.gameOver) {
			gameManager.Score (10);
		}
	}
}

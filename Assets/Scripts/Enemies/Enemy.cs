using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	private PlayerController playerCont;
	public float approachSpeed;
	public float avoidSpeed;
	public float zagSpeed;
	public float coneFactor;
	private Rigidbody2D rb;
	protected float zagTime;
	protected float timeUntilZag;
	public int id;
	private int health;
	protected int startingHealth;


	// Use this for initialization
	void Start () {
		coneFactor = 0.5f;
		rb = GetComponent<Rigidbody2D> ();
		startingHealth = 2;
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Services.GameManager.gameOver) {
			Move ();
		}
	}

	protected virtual void Initialize() {
		health = startingHealth;
	}

	protected virtual void Move() {}

	protected void ApproachPlayer(){
		rb.MovePosition(Vector3.MoveTowards (transform.position, Services.GameManager.player.transform.position, approachSpeed * Time.deltaTime));
	}

	protected void AvoidPlayer(){
		rb.MovePosition (Vector3.MoveTowards (transform.position, Services.GameManager.player.transform.position, -avoidSpeed * Time.deltaTime));
	}

	protected void Zag(){
		Vector3 vectorFromMeToPlayer = (Services.GameManager.player.transform.position - transform.position);
		float angle = Mathf.Rad2Deg * Mathf.Atan2 (vectorFromMeToPlayer.y, vectorFromMeToPlayer.x);
		float zagAngle = Mathf.Deg2Rad * (angle + 30);
		Vector3 zagVector = new Vector3 (Mathf.Cos (zagAngle), Mathf.Sin (zagAngle));
		rb.velocity = zagVector * zagSpeed;
	}

	protected bool IsPlayerFacingMe(){
		float angleInDegrees = Mathf.Deg2Rad * (Services.GameManager.player.transform.eulerAngles.z + 90);
		Vector3 playerRotationVector = new Vector3 (Mathf.Cos (angleInDegrees), Mathf.Sin (angleInDegrees)).normalized;
		Vector3 vectorFromPlayerToMe = (transform.position - Services.GameManager.player.transform.position).normalized;
		bool isFacing = Vector3.Dot (playerRotationVector, vectorFromPlayerToMe) > coneFactor;

		return isFacing;
	}

	protected void SetDeathClip(AudioClip deathClip){
		GetComponent<AudioSource> ().clip = deathClip;
	}

	public virtual void TakeDamage(int damage){
		health -= damage;
		if (health <= 0) {
			Die ();
		}
	}

	protected virtual void Die(){
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<CircleCollider2D> ().enabled = false;
		GetComponent<AudioSource> ().Play ();
		float audioLength = GetComponent<AudioSource> ().clip.length;
		if (!Services.GameManager.gameOver) {
			Services.GameManager.Score (10);
		}
		Services.EnemyManager.DestroyEnemy (this, audioLength);
	}
}

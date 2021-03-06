﻿using UnityEngine;
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
	public int health;
	public int startingHealth;
	public int pointValue;
	public bool frozen;


	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Services.MainGame.gameOver && !frozen) {
			Move ();
			SpecialUpdate ();
		}
	}

	protected virtual void Initialize() {
		SetValues ();
		rb = GetComponent<Rigidbody2D> ();
		health = startingHealth;
		frozen = false;
	}

	protected virtual void SetValues(){
		startingHealth = 2;
		pointValue = 10;
		coneFactor = 45;
	}

	protected virtual void SpecialUpdate() {}

	protected virtual void Move() {}

	public void ApproachPlayer(){
		rb.MovePosition(Vector3.MoveTowards (transform.position, Services.MainGame.player.transform.position, approachSpeed * Time.deltaTime));
	}

	protected void AvoidPlayer(){
		rb.MovePosition (Vector3.MoveTowards (transform.position, Services.MainGame.player.transform.position, -avoidSpeed * Time.deltaTime));
	}

	protected void Zag(){
		Vector3 vectorFromMeToPlayer = (Services.MainGame.player.transform.position - transform.position);
		float angle = Mathf.Rad2Deg * Mathf.Atan2 (vectorFromMeToPlayer.y, vectorFromMeToPlayer.x);
		float zagAngle = Mathf.Deg2Rad * (angle + 30);
		Vector3 zagVector = new Vector3 (Mathf.Cos (zagAngle), Mathf.Sin (zagAngle));
		rb.velocity = zagVector * zagSpeed;
	}

	protected bool IsPlayerFacingMe(){
		float angleInDegrees = Mathf.Deg2Rad * (Services.MainGame.player.transform.eulerAngles.z + 90);
		Vector3 playerRotationVector = new Vector3 (Mathf.Cos (angleInDegrees), Mathf.Sin (angleInDegrees)).normalized;
		Vector3 vectorFromPlayerToMe = (transform.position - Services.MainGame.player.transform.position).normalized;
		bool isFacing = Mathf.Acos(Vector3.Dot (playerRotationVector, vectorFromPlayerToMe)) < (coneFactor * Mathf.Deg2Rad);

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

	protected float PlayDeathSound(){
		GetComponent<AudioSource> ().Play ();
		return GetComponent<AudioSource> ().clip.length;
	}

	protected void Disable(){
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer> ();
		Collider2D[] cols = GetComponentsInChildren<Collider2D> ();
		foreach (SpriteRenderer sr in srs) {
			sr.enabled = false;
		}
		foreach (Collider2D col in cols) {
			col.enabled = false;
		}
	}

	protected virtual void Die(){
		Disable ();
		float audioLength = PlayDeathSound ();
		Services.EnemyManager.DestroyEnemy (this, audioLength);
	}

	public void Freeze(){
		frozen = true;
		rb.velocity = Vector3.zero;
		GetComponent<Collider2D> ().enabled = false;
	}
}

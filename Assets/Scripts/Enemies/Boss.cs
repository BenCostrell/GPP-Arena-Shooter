﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {

	public float spinRate;
	private GameObject ring;

	protected override void Initialize ()
	{
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Art/bossCenter");

		ring = new GameObject ();
		SpriteRenderer ringSr = ring.AddComponent<SpriteRenderer> ();
		ringSr.sprite = Resources.Load<Sprite> ("Art/bossRing");
		ring.transform.SetParent (transform, false);
	
		CircleCollider2D col = GetComponent<CircleCollider2D> ();
		col.radius = 1;

		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);

		base.Initialize ();
	}

	protected override void SetValues ()
	{
		base.SetValues ();
		approachSpeed = 2f;
		startingHealth = 100;
		spinRate = 0;
	}

	protected override void SpecialUpdate ()
	{
		ring.transform.Rotate (Vector3.forward, spinRate);
	}

	public override void TakeDamage(int damage){
		base.TakeDamage (damage);
		UpdateHealthUI ();
	}

	protected void UpdateHealthUI(){
		float healthfloat = health;
		Services.GameManager.bossHealthBar.transform.localScale = new Vector3 (1, healthfloat / startingHealth, 1);
	}

	protected override void Die ()
	{
		Disable ();
		PlayDeathSound ();
	}

	public void SpawnEnemies(){
		float spawnDistanceFromBoss = 8f;
		float angle = 0f;
		Vector3 relativeSpawnLocation;
		for (int i = 0; i < 5; i++) {
			relativeSpawnLocation = new Vector3 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad), 0) * spawnDistanceFromBoss;
			Services.EnemyManager.GenerateEnemy (relativeSpawnLocation + transform.position, Services.EnemyManager.RandomEnemyType ());
			angle -= 45;
		}
	}
}

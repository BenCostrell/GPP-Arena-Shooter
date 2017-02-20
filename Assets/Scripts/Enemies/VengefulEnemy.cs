using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulEnemy : Enemy {

	protected override void Initialize(){
		approachSpeed = 1f;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/vengefulEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);
		enemyManager.gameManager.eventManager.Register<EnemyDied> (GainSpeed);
	}

	protected void GainSpeed(EnemyDied e){
		approachSpeed += 1.5f;
		Debug.Log ("getting faster - " + approachSpeed);
	}

	protected override void Move(){
		ApproachPlayer ();
	}

	public override void Die(){
		enemyManager.gameManager.eventManager.Unregister<EnemyDied> (GainSpeed);
		base.Die ();
	}

}

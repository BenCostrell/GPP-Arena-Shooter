using UnityEngine;
using System.Collections;

public class BoldEnemy : Enemy {

	protected override void Initialize(){
		approachSpeed = 6f;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/boldEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);
		base.Initialize ();
	}

	protected override void Move(){
		ApproachPlayer ();
	}
}

using UnityEngine;
using System.Collections;

public class ShyEnemy : Enemy {

	protected override void Initialize(){
		approachSpeed = 5f;
		avoidSpeed = 1f;

		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/shyEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_android8") as AudioClip;
		SetDeathClip (deathClip);
	}

	protected override void Move(){
		if (IsPlayerFacingMe ()) {
			AvoidPlayer ();
		} else {
			ApproachPlayer ();
		}
	}
}

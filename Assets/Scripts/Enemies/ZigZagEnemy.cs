using UnityEngine;
using System.Collections;

public class ZigZagEnemy : Enemy {

	protected override void Initialize(){
		approachSpeed = 5f;
		zagSpeed = 8f;
		zagTime = 0.5f;
		timeUntilZag = 0;

		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/zigZagEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_android2") as AudioClip;
		SetDeathClip (deathClip);
		base.Initialize ();
	}

	protected override void Move(){
		if (timeUntilZag > zagTime) {
			timeUntilZag -= Time.deltaTime;
		} else if (timeUntilZag > 0) {
			ApproachPlayer ();
			timeUntilZag -= Time.deltaTime;
		} else {
			Zag ();
			timeUntilZag = zagTime * 2;
		}
	}
}

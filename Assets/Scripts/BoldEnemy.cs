using UnityEngine;
using System.Collections;

public class BoldEnemy : Enemy {

	protected override void Initialize(){
		approachSpeed = 3f;

		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);
	}

	protected override void Move(){
		ApproachPlayer ();
	}
}

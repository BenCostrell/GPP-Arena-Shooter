using UnityEngine;
using System.Collections;

public class BoldEnemy : Enemy {

	protected override void Initialize(){
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/boldEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);
		base.Initialize ();
	}

	protected override void SetValues ()
	{
		base.SetValues ();
		approachSpeed = 6f;
	}

	protected override void Move(){
		ApproachPlayer ();
	}
}

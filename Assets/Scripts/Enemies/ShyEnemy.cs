using UnityEngine;
using System.Collections;

public class ShyEnemy : Enemy {

	protected override void Initialize(){
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/shyEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_android8") as AudioClip;
		SetDeathClip (deathClip);
		base.Initialize ();
	}

	protected override void SetValues ()
	{
		base.SetValues ();
		approachSpeed = 10f;
		avoidSpeed = 2f;
	}

	protected override void Move(){
		if (IsPlayerFacingMe ()) {
			AvoidPlayer ();
		} else {
			ApproachPlayer ();
		}
	}
}

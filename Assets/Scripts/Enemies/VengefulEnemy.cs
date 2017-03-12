using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulEnemy : Enemy {

	protected override void Initialize(){
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Art/vengefulEnemy");
		AudioClip deathClip = Resources.Load ("Sounds/sfx_deathscream_robot1") as AudioClip;
		SetDeathClip (deathClip);
		Services.EventManager.Register<EnemyDied> (GainSpeed);
		base.Initialize ();
	}

	protected override void SetValues ()
	{
		base.SetValues ();
		approachSpeed = 2f;
	}

	protected void GainSpeed(EnemyDied e){
		approachSpeed += 3f;
	}

	protected override void Move(){
		ApproachPlayer ();
	}

	protected override void Die(){
		Services.EventManager.Unregister<EnemyDied> (GainSpeed);
		base.Die ();
	}

}

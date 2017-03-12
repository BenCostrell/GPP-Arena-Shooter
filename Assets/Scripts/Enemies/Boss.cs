using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
	protected override void Initialize ()
	{
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Art/bossCenter");

		GameObject ring = new GameObject ();
		SpriteRenderer ringSr = ring.AddComponent<SpriteRenderer> ();
		ringSr.sprite = Resources.Load<Sprite> ("Art/bossRing");
		ring.transform.SetParent (transform, false);

		transform.localScale = 2 * Vector3.one;

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
	}

	protected override void Die ()
	{
		Disable ();
		PlayDeathSound ();
	}
}

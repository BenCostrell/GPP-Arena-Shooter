using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
	protected override void Initialize ()
	{
		approachSpeed = 2f;
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Art/bossCenter");
		GameObject ring = new GameObject ();
		SpriteRenderer ringSr = ring.AddComponent<SpriteRenderer> ();
		ringSr.sprite = Resources.Load<Sprite> ("Art/bossRing");
		ring.transform.SetParent (transform);
		base.Initialize ();
	}
}

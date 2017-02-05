using UnityEngine;
using System.Collections;

public class ShyEnemy : Enemy {

	protected override void InitializeSpeed(){
		approachSpeed = 5f;
		avoidSpeed = 1f;
	}

	protected override void Move(){
		if (IsPlayerFacingMe ()) {
			AvoidPlayer ();
		} else {
			ApproachPlayer ();
		}
	}
}

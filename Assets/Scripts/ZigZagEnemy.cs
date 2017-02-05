using UnityEngine;
using System.Collections;

public class ZigZagEnemy : Enemy {

	protected override void InitializeSpeed(){
		approachSpeed = 5f;
		zagSpeed = 8f;
		zagTime = 0.5f;
		timeUntilZag = 0;
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

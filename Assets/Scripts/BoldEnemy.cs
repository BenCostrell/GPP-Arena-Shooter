using UnityEngine;
using System.Collections;

public class BoldEnemy : Enemy {
	public BoldEnemy(float approachSpd){
		approachSpeed = approachSpd;

	}

	protected override void Move(){
		ApproachPlayer ();
	}
}

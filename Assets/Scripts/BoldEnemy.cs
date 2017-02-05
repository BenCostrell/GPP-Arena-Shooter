using UnityEngine;
using System.Collections;

public class BoldEnemy : Enemy {

	protected override void InitializeSpeed(){
		approachSpeed = 3f;
	}

	protected override void Move(){
		ApproachPlayer ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireMode : Task {
	private float timeSinceLastShot;

	protected override void Init ()
	{
		Services.EnemyManager.boss.FireRandomly ();
		timeSinceLastShot = 0;
		Services.EnemyManager.boss.spinRate = 2;
	}

	internal override void Update ()
	{
		Boss boss = Services.EnemyManager.boss;
		if (timeSinceLastShot > boss.fireRate) {
			boss.FireRandomly ();
			timeSinceLastShot = 0;
		} else {
			timeSinceLastShot += Time.deltaTime;
		}

		if (boss.health <= boss.startingHealth * 0.15f) {
			SetStatus (TaskStatus.Success);
		}
	}


}

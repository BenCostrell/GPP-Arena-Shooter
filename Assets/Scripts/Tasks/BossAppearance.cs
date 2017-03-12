using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppearance : Task {

	private Boss boss;

	protected override void Init () {
		boss = Services.EnemyManager.SpawnBoss ();
	}
}

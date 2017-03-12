using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseMode : Task {
	protected override void Init ()
	{
		Services.EnemyManager.boss.SpawnEnemies ();
		Services.EventManager.Register<WaveCleared> (OnWaveCleared);
		Services.EnemyManager.boss.spinRate = 3;
	}

	internal override void Update ()
	{
		Boss boss = Services.EnemyManager.boss;

		boss.ApproachPlayer ();

		if (boss.health == 0) {
			SetStatus (TaskStatus.Success);
		}
	}

	internal void OnWaveCleared (WaveCleared e){
		Services.EnemyManager.boss.SpawnEnemies ();
	}

	protected override void OnSuccess ()
	{
		Services.EventManager.Unregister<WaveCleared> (OnWaveCleared);
	}
}

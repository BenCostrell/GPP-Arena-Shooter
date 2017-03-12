using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnMode : Task {
	protected override void Init ()
	{
		Services.EnemyManager.boss.SpawnEnemies ();
		Services.EventManager.Register<WaveCleared> (OnWaveCleared);
	}

	internal override void Update ()
	{
		if (Services.EnemyManager.boss.health <= Services.EnemyManager.boss.startingHealth / 2) {
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

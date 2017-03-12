﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppearance : Task {

	private float timeElapsed;

	protected override void Init () {
		Services.EnemyManager.SpawnBoss ();
	}

	internal override void Update ()
	{
		Boss boss = Services.EnemyManager.boss;
		Vector3 targetSize = Services.EnemyManager.bossSize * Vector3.one;
		boss.transform.localScale = Vector3.Lerp (Vector3.zero, targetSize, timeElapsed / Services.GameManager.appearanceDuration);
		timeElapsed += Time.deltaTime;

		if (boss.transform.localScale == targetSize) {
			SetStatus (TaskStatus.Success);
		}
	}

	protected override void OnSuccess ()
	{
		Services.GameManager.player.GetComponent<PlayerController> ().EnableInput ();
	}
}
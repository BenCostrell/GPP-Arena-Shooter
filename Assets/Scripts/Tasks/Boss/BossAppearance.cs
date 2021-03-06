﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppearance : Task {

	private float timeElapsed;

	protected override void Init () {
		Services.EnemyManager.SpawnBoss ();
		Services.MainGame.bossHealthBack.SetActive (true);
		Services.MainGame.bossHealthBack.transform.localScale = Vector3.one;
		Services.MainGame.bossHealthBar.transform.localScale = Vector3.one;
	}

	internal override void Update ()
	{
		Boss boss = Services.EnemyManager.boss;
		Vector3 targetSize = Services.EnemyManager.bossSize * Vector3.one;
		float duration = Services.MainGame.appearanceDuration;
		boss.transform.localScale = Vector3.Lerp (Vector3.zero, targetSize, Easing.QuartEaseOut(timeElapsed / duration));
		Services.MainGame.bossHealthBar.transform.localScale = Vector3.Lerp (new Vector3 (1, 0, 1), Vector3.one, 
			Easing.QuartEaseOut (timeElapsed / duration));
		timeElapsed = Mathf.Min(duration, timeElapsed + Time.deltaTime);

		if (boss.transform.localScale == targetSize) {
			SetStatus (TaskStatus.Success);
		}
	}

	protected override void OnSuccess ()
	{
		Services.MainGame.player.GetComponent<PlayerController> ().EnableInput ();
	}
}

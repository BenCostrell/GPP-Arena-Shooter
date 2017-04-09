using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnding : Task {
	private float timeElapsed;
	private Vector3 playerLocationAtEnd;

	protected override void Init ()
	{
		Services.MainGame.player.GetComponent<PlayerController> ().DisableInput ();
		Services.EnemyManager.FreezeAllEnemies ();
		Services.EnemyManager.boss.Freeze ();
		playerLocationAtEnd = Services.MainGame.player.transform.position;
	}

	internal override void Update ()
	{
		float duration = Services.MainGame.endingDuration;
		Boss boss = Services.EnemyManager.boss;

		Services.MainGame.bossHealthBack.transform.localScale = 
			Vector3.Lerp (Vector3.one, new Vector3(1, 0, 1), Easing.QuartEaseOut(timeElapsed / duration));
		boss.transform.localScale = Vector3.Lerp (Services.EnemyManager.bossSize * Vector3.one, Vector3.zero, 
			Easing.QuartEaseOut (timeElapsed / duration));
		Services.EnemyManager.ShrinkAllEnemies (Vector3.Lerp (0.5f * Vector3.one, Vector3.zero, Easing.QuartEaseOut (timeElapsed / duration)));
		Services.MainGame.player.transform.position = Vector3.Lerp (playerLocationAtEnd, Vector3.zero, Easing.QuartEaseOut (timeElapsed / duration));
		timeElapsed = Mathf.Min(duration, timeElapsed + Time.deltaTime);

		if (boss.transform.localScale == Vector3.zero) {
			SetStatus (TaskStatus.Success);
		}
	}

	protected override void OnSuccess ()
	{
		Services.EnemyManager.DestroyAllEnemies ();
		Services.MainGame.bossHealthBack.SetActive (false);
		Services.EnemyManager.boss.DestroyThis ();
		Services.EnemyManager.ResumeAutoRespawning ();
		Services.MainGame.player.GetComponent<PlayerController> ().EnableInput ();
		Services.EnemyManager.SpawnNewWave ();
	}
}

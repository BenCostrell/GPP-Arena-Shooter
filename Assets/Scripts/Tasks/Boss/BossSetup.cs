using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetup : Task {

	private float timeElapsed;
	private Vector3 initialPlayerPosition;
	private Vector3 initialPlayerRotation;

	protected override void Init ()
	{
		GameObject player = Services.MainGame.player;
		Services.MainGame.bossBattleMessage.SetActive (true);
		timeElapsed = 0;
		initialPlayerPosition = player.transform.position;
		initialPlayerRotation = player.transform.rotation.eulerAngles;
		player.GetComponent<PlayerController> ().DisableInput ();
		Services.EnemyManager.PauseAutoRespawning ();
	}

	internal override void Update ()
	{
		GameObject player = Services.MainGame.player;
		float duration = Services.MainGame.setupDuration;
		Vector3 targetPosition = Services.MainGame.playerPositionBeforeBossBattle;

		player.transform.position = Vector3.Lerp (initialPlayerPosition, targetPosition, Easing.QuadEaseOut(timeElapsed / duration));
		player.transform.rotation = Quaternion.Euler (Vector3.Lerp (initialPlayerRotation, Vector3.zero, Easing.QuadEaseOut(timeElapsed / duration)));
		Services.MainGame.bossBattleMessage.transform.localScale = Vector3.LerpUnclamped (Vector3.zero, Vector3.one, 
			Easing.BackEaseOut (timeElapsed / duration));
		
		timeElapsed = Mathf.Min(duration, timeElapsed + Time.deltaTime);

		if (player.transform.position == targetPosition && player.transform.rotation == Quaternion.identity) {
			SetStatus (TaskStatus.Success);
		}
	}

	protected override void OnSuccess ()
	{
		Services.MainGame.bossBattleMessage.SetActive (false);
	}
}

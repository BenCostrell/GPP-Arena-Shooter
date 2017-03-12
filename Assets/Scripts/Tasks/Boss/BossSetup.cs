using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSetup : Task {

	private float timeElapsed;
	private Vector3 initialPlayerPosition;
	private Vector3 initialPlayerRotation;

	protected override void Init ()
	{
		GameObject player = Services.GameManager.player;
		Services.GameManager.bossBattleMessage.SetActive (true);
		timeElapsed = 0;
		initialPlayerPosition = player.transform.position;
		initialPlayerRotation = player.transform.rotation.eulerAngles;
		player.GetComponent<PlayerController> ().DisableInput ();
		player.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
	}

	internal override void Update ()
	{
		GameObject player = Services.GameManager.player;
		float duration = Services.GameManager.setupDuration;
		Vector3 targetPosition = Services.GameManager.playerPositionBeforeBossBattle;

		player.transform.position = Vector3.Lerp (initialPlayerPosition, targetPosition, Easing.QuadEaseOut(timeElapsed / duration));
		player.transform.rotation = Quaternion.Euler (Vector3.Lerp (initialPlayerRotation, Vector3.zero, Easing.QuadEaseOut(timeElapsed / duration)));
		Services.GameManager.bossBattleMessage.transform.localScale = Vector3.LerpUnclamped (Vector3.zero, Vector3.one, 
			Easing.BackEaseOut (timeElapsed / duration));
		
		timeElapsed = Mathf.Min(duration, timeElapsed + Time.deltaTime);

		if (player.transform.position == targetPosition && player.transform.rotation == Quaternion.identity) {
			SetStatus (TaskStatus.Success);
		}
	}

	protected override void OnSuccess ()
	{
		Services.GameManager.bossBattleMessage.SetActive (false);
	}
}

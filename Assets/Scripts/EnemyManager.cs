using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public GameObject enemyPrefab;
	public int initialNumEnemiesPerWave;
	public int wavesBeforeIncrease;
	private int waveCount;
	private int enemiesToSpawn;
	private List<Enemy> enemyList;
	public GameManager gameManager;
	public enum EnemyType {Bold, Shy, ZigZag, Vengeful};
	private List<EnemyType> enemyTypes;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ();
		enemiesToSpawn = initialNumEnemiesPerWave;
		waveCount = 1;
		enemyList = new List<Enemy> ();
		enemyTypes = new List<EnemyType> (){ EnemyType.Bold, EnemyType.Shy, EnemyType.ZigZag, EnemyType.Vengeful };
	}

	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			if (enemyList.Count == 0) {
				GenerateWave (enemiesToSpawn);
				waveCount += 1;
				if (waveCount % wavesBeforeIncrease == 0) {
					enemiesToSpawn += 1;
				}
			}
		}
	}		

	void GenerateEnemy(Vector3 location, EnemyType type){
		GameObject newEnemy = Instantiate (enemyPrefab, location, Quaternion.identity) as GameObject;
		if (type == EnemyType.Bold) {
			newEnemy.AddComponent<BoldEnemy> ();
		} else if (type == EnemyType.Shy) {
			newEnemy.AddComponent<ShyEnemy> ();
		} else if (type == EnemyType.ZigZag) {
			newEnemy.AddComponent<ZigZagEnemy> ();
		} else if (type == EnemyType.Vengeful){
			newEnemy.AddComponent<VengefulEnemy>();
		}
		enemyList.Add (newEnemy.GetComponent<Enemy> ());
	}

	void GenerateWave(int numEnemies){
		for (int i = 0; i < numEnemies; i++) {
			float xCoord;
			float yCoord;
			float pointOnUnfoldedRectangle = Random.Range (0, 104);
			if (pointOnUnfoldedRectangle < 32) {
				xCoord = -16 + pointOnUnfoldedRectangle;
				yCoord = 10;
			} else if (pointOnUnfoldedRectangle < 52) {
				xCoord = 16;
				yCoord = -10 + pointOnUnfoldedRectangle - 32;
			} else if (pointOnUnfoldedRectangle < 84) {
				xCoord = -16 + pointOnUnfoldedRectangle - 52;
				yCoord = -10;
			} else {
				xCoord = -16;
				yCoord = -10 + pointOnUnfoldedRectangle - 84;
			}


			int typeNum = Random.Range (0, enemyTypes.Count);
			EnemyType type = enemyTypes [typeNum];

			GenerateEnemy (new Vector3 (xCoord, yCoord, 0), type);
		}
	}

	public void DestroyEnemy(Enemy enemy, float timeToDestroy){
		Destroy (enemy.gameObject, timeToDestroy);
		gameManager.eventManager.Fire (new EnemyDied());
		enemyList.Remove (enemy);
	}
}

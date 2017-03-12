using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public int initialNumEnemiesPerWave;
	public int wavesBeforeIncrease;
	public int wavesBeforeBoss;
	private int waveCount;
	private int enemiesToSpawn;
	public float spawnRectWidth;
	public float spawnRectHeight;
	public Vector3 bossSpawnLocation;
	private List<Enemy> enemyList;
	public enum EnemyType {Bold, Shy, ZigZag, Vengeful, Boss};
	private List<EnemyType> spawnableEnemyTypes;

	// Use this for initialization
	void Start () {
		enemiesToSpawn = initialNumEnemiesPerWave;
		waveCount = 1;
		enemyList = new List<Enemy> ();
		spawnableEnemyTypes = new List<EnemyType> (){ EnemyType.Bold, EnemyType.Shy, EnemyType.ZigZag, EnemyType.Vengeful };

		Services.EventManager.Register<WaveCleared> (OnWaveCleared);
		Services.EventManager.Register<GameOver> (OnGameOver);

		SpawnNewWave ();
	}

	// Update is called once per frame
	void Update () {
	}		


	void OnGameOver(GameOver e){
		Services.EventManager.Unregister<WaveCleared> (OnWaveCleared);
		Services.EventManager.Unregister<GameOver> (OnGameOver);
	}

	void SpawnNewWave(){
		GenerateWave (enemiesToSpawn);
	}

	void OnWaveCleared(WaveCleared e){
		waveCount += 1;
		if (waveCount % wavesBeforeIncrease == 0) {
			enemiesToSpawn += 1;
		}
		if (waveCount % wavesBeforeBoss == 0) {
			BossAppearance bossAppearance = new BossAppearance ();
			Services.TaskManager.AddTask (bossAppearance);
		} else {
			SpawnNewWave ();
		}
	}

	void GenerateEnemy(Vector3 location, EnemyType type){
		GameObject newEnemy = Instantiate (Services.PrefabDB.enemy, location, Quaternion.identity) as GameObject;
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
			float pointOnUnfoldedRectangle = Random.Range (0, 2 * (spawnRectWidth + spawnRectHeight));
			if (pointOnUnfoldedRectangle < spawnRectWidth) {
				xCoord = -spawnRectWidth/2 + pointOnUnfoldedRectangle;
				yCoord = spawnRectHeight/2;
			} else if (pointOnUnfoldedRectangle < spawnRectWidth + spawnRectHeight) {
				xCoord = spawnRectWidth/2;
				yCoord = -spawnRectHeight/2 + pointOnUnfoldedRectangle - spawnRectWidth;
			} else if (pointOnUnfoldedRectangle < ((2 * spawnRectWidth) + spawnRectHeight)) {
				xCoord = -spawnRectWidth/2 + pointOnUnfoldedRectangle - (spawnRectWidth +spawnRectHeight);
				yCoord = -spawnRectHeight/2;
			} else {
				xCoord = -spawnRectWidth/2;
				yCoord = -spawnRectHeight / 2 + pointOnUnfoldedRectangle - ((2 * spawnRectWidth) + spawnRectHeight);
			}


			int typeNum = Random.Range (0, spawnableEnemyTypes.Count);
			EnemyType type = spawnableEnemyTypes [typeNum];

			GenerateEnemy (new Vector3 (xCoord, yCoord, 0), type);
		}
	}

	public void DestroyEnemy(Enemy enemy, float timeToDestroy){
		Destroy (enemy.gameObject, timeToDestroy);
		Services.EventManager.Fire (new EnemyDied(enemy));
		enemyList.Remove (enemy);
		if (enemyList.Count == 0) {
			Services.EventManager.Fire (new WaveCleared ());
		}
	}

	public Boss SpawnBoss(){
		GameObject bossObj = Instantiate (Services.PrefabDB.enemy, bossSpawnLocation, Quaternion.identity);
		Boss boss = bossObj.AddComponent<Boss> ();
		return boss;
	}
}

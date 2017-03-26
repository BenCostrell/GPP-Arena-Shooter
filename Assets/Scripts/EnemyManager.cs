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
	public enum EnemyType {Bold, Shy, ZigZag, Vengeful, Smart, Boss};
	private List<EnemyType> spawnableEnemyTypes;
	public Boss boss;
	public float bossSize;

	// Use this for initialization
	void Start () {
		enemiesToSpawn = initialNumEnemiesPerWave;
		waveCount = 1;
		enemyList = new List<Enemy> ();
		spawnableEnemyTypes = new List<EnemyType> (){ EnemyType.Bold, EnemyType.Shy, EnemyType.ZigZag, EnemyType.Vengeful, EnemyType.Smart };

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

	public void SpawnNewWave(){
		GenerateWave (enemiesToSpawn);
	}

	void OnWaveCleared(WaveCleared e){
		waveCount += 1;
		if (waveCount % wavesBeforeIncrease == 0) {
			enemiesToSpawn += 1;
		}
		if (waveCount % wavesBeforeBoss == 0) {
			Services.GameManager.ItsBossTime ();
		} else {
			SpawnNewWave ();
		}
	}

	public void ResumeAutoRespawning(){
		Services.EventManager.Register<WaveCleared> (OnWaveCleared);
	}

	public void PauseAutoRespawning(){
		Services.EventManager.Unregister<WaveCleared> (OnWaveCleared);
	}

	public void GenerateEnemy(Vector3 location, EnemyType type){
		GameObject newEnemy = Instantiate (Services.PrefabDB.Enemy, location, Quaternion.identity) as GameObject;
        switch (type)
        {
            case EnemyType.Bold:
                newEnemy.AddComponent<BoldEnemy>();
                break;
            case EnemyType.Shy:
                newEnemy.AddComponent<ShyEnemy>();
                break;
            case EnemyType.ZigZag:
                newEnemy.AddComponent<ZigZagEnemy>();
                break;
            case EnemyType.Vengeful:
                newEnemy.AddComponent<VengefulEnemy>();
                break;
            case EnemyType.Smart:
                newEnemy.AddComponent<SmartEnemy>();
                break;
            default:
                break;
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
				
			GenerateEnemy (new Vector3 (xCoord, yCoord, 0), RandomEnemyType());
		}
	}

	public EnemyType RandomEnemyType(){
		int typeNum = Random.Range (0, spawnableEnemyTypes.Count);
		return spawnableEnemyTypes [typeNum];;
	}

	public void DestroyEnemy(Enemy enemy, float timeToDestroy){
		Services.EventManager.Fire (new EnemyDied(enemy));
		enemyList.Remove (enemy);
		Destroy (enemy.gameObject, timeToDestroy);
		if (enemyList.Count == 0) {
			Services.EventManager.Fire (new WaveCleared ());
		}
	}

	public void SpawnBoss(){
		GameObject bossObj = Instantiate (Services.PrefabDB.Enemy, bossSpawnLocation, Quaternion.identity);
		boss = bossObj.AddComponent<Boss> ();
	}

	public void FreezeAllEnemies(){
		foreach (Enemy enemy in enemyList) {
			enemy.Freeze ();
		}
	}

	public void DestroyAllEnemies(){
		for (int i = enemyList.Count - 1; i >= 0; i--) {
			DestroyEnemy (enemyList[i], 0);
		}
	}

	public void ShrinkAllEnemies(Vector3 size){
		foreach (Enemy enemy in enemyList) {
			enemy.transform.localScale = size;
		}
	}
}

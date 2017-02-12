using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public GameObject enemyPrefab;
	public Sprite boldEnemySprite;
	public Sprite shyEnemySprite;
	public Sprite zigZagEnemySprite;
	public int initialNumEnemiesPerWave;
	public int wavesBeforeIncrease;
	private int waveCount;
	private int enemiesToSpawn;
	private List<Enemy> enemyList;
	private GameManager gameManager;
	public enum EnemyType {Bold, Shy, ZigZag};

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ();
		enemiesToSpawn = initialNumEnemiesPerWave;
		waveCount = 1;
		enemyList = new List<Enemy> ();
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
		SpriteRenderer sr = newEnemy.GetComponent<SpriteRenderer> ();
		if (type == EnemyType.Bold) {
			BoldEnemy boldEnemy = newEnemy.AddComponent<BoldEnemy> ();
			sr.sprite = boldEnemySprite;
		} else if (type == EnemyType.Shy) {
			ShyEnemy shyEnemy = newEnemy.AddComponent<ShyEnemy> ();
			sr.sprite = shyEnemySprite;
		} else if (type == EnemyType.ZigZag) {
			ZigZagEnemy zigZagEnemy = newEnemy.AddComponent<ZigZagEnemy> ();
			sr.sprite = zigZagEnemySprite;
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

			EnemyType type;
			int typeNum = Random.Range (0, 3);
			if (typeNum == 0) {
				type = EnemyType.Bold;
			} else if (typeNum == 1) {
				type = EnemyType.Shy;
			} else {
				type = EnemyType.ZigZag;
			}

			GenerateEnemy (new Vector3 (xCoord, yCoord, 0), type);
		}
	}

	public void DestroyEnemy(Enemy enemy, float timeToDestroy){
		Destroy (enemy.gameObject, timeToDestroy);
		enemyList.Remove (enemy);
	}
}

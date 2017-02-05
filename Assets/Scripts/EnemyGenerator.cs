using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	public GameObject enemyPrefab;
	public float boldEnemyApproachSpeed;
	public Sprite boldEnemySprite;
	public Sprite shyEnemySprite;
	private float timeUntilNextWave;
	public float timeBetweenWaves;
	private GameManager gameManager;
	public enum EnemyType {Bold, Shy};

	// Use this for initialization
	void Start () {
		timeUntilNextWave = 0;
		gameManager = GameObject.FindWithTag ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			if (timeUntilNextWave > 0) {
				timeUntilNextWave -= Time.deltaTime;
			} else {
				GenerateWave (5);
				timeUntilNextWave = timeBetweenWaves;
			}
		}
	}		

	void GenerateEnemy(Vector3 location, EnemyType type){
		GameObject newEnemy = Instantiate (enemyPrefab, location, Quaternion.identity) as GameObject;
		if (type == EnemyType.Bold) {
			BoldEnemy boldEnemy = newEnemy.AddComponent<BoldEnemy> ();
			newEnemy.GetComponent<SpriteRenderer> ().sprite = boldEnemySprite;
		} else if (type == EnemyType.Shy) {
			ShyEnemy shyEnemy = newEnemy.AddComponent<ShyEnemy> ();
			newEnemy.GetComponent<SpriteRenderer> ().sprite = shyEnemySprite;
		}
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
			int typeNum = Random.Range (0, 2);
			if (typeNum == 0) {
				type = EnemyType.Bold;
			} else {
				type = EnemyType.Shy;
			}

			GenerateEnemy (new Vector3 (xCoord, yCoord, 0), type);
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	public GameObject enemyPrefab;
	public float boldEnemyApproachSpeed;
	public Sprite boldEnemySprite;
	private float timeUntilNextWave;
	public float timeBetweenWaves;

	// Use this for initialization
	void Start () {
		timeUntilNextWave = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilNextWave > 0) {
			timeUntilNextWave -= Time.deltaTime;
		} else {
			GenerateWave (5);
			timeUntilNextWave = timeBetweenWaves;
		}
	}

	void GenerateBoldEnemy(Vector3 location){
		GameObject newEnemy = Instantiate (enemyPrefab, location, Quaternion.identity) as GameObject;
		BoldEnemy boldEnemy = newEnemy.AddComponent<BoldEnemy> ();
		newEnemy.GetComponent<SpriteRenderer> ().sprite = boldEnemySprite;
		boldEnemy.approachSpeed = boldEnemyApproachSpeed;
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
			GenerateBoldEnemy (new Vector3 (xCoord, yCoord, 0));
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public GameObject gameOverMessage;
	public bool gameOver;
	public int score;
	public GameObject scoreText;
	public GameObject player;

	// Use this for initialization
	void Start () {
		gameOverMessage.SetActive (false);
		gameOver = false;
		Services.EventManager = new EventManager ();
		Services.EnemyManager = GameObject.FindGameObjectWithTag ("EnemyManager").GetComponent<EnemyManager> ();
		Services.GameManager = this;
		Services.PrefabDB = GameObject.FindGameObjectWithTag ("PrefabDB").GetComponent<PrefabDB> ();

		InitializePlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset")){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void EndGame(){
		gameOverMessage.SetActive (true);
		gameOver = true;
	}

	public void Score(int points){
		score += points;
		scoreText.GetComponent<Text> ().text = score.ToString ();
	}

	private void InitializePlayer(){
		player = Instantiate (Services.PrefabDB.player, Vector3.zero, Quaternion.identity) as GameObject;
	}
}

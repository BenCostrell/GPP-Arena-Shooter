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
		InitializeServices ();

		Services.EventManager.Register<EnemyDied> (Score);

		InitializePlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset")){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		Services.TaskManager.Update ();
	}

	void InitializeServices(){
		Services.EventManager = new EventManager ();
		Services.EnemyManager = GameObject.FindGameObjectWithTag ("EnemyManager").GetComponent<EnemyManager> ();
		Services.GameManager = this;
		Services.PrefabDB = GameObject.FindGameObjectWithTag ("PrefabDB").GetComponent<PrefabDB> ();
		Services.TaskManager = new TaskManager ();
	}

	public void EndGame(){
		gameOverMessage.SetActive (true);
		gameOver = true;
		Services.EventManager.Unregister<EnemyDied> (Score);
		Services.EventManager.Fire (new GameOver ());
	}

	public void Score(EnemyDied e){
		score += e.enemyThatDied.pointValue;
		scoreText.GetComponent<Text> ().text = score.ToString ();
	}

	private void InitializePlayer(){
		player = Instantiate (Services.PrefabDB.player, Vector3.zero, Quaternion.identity) as GameObject;
	}
}

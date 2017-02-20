using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public GameObject gameOverMessage;
	public bool gameOver;
	public int score;
	public GameObject scoreText;
	public EventManager eventManager;

	// Use this for initialization
	void Start () {
		gameOverMessage.SetActive (false);
		gameOver = false;
		eventManager = EventManager.Instance;
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
}

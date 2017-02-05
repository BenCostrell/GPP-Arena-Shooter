using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public GameObject gameOverMessage;
	public bool gameOver;

	// Use this for initialization
	void Start () {
		gameOverMessage.SetActive (false);
		gameOver = false;
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
}

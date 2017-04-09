using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    public GameObject sceneRoot;

	void Awake()
    {
        InitializeServices();
    }
	void Start () {
        Services.SceneStackManager.PushScene<TitleScreen>();
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
		Services.GameManager = this;
		Services.PrefabDB = Resources.Load<PrefabDB> ("Prefabs/PrefabDB");
		Services.TaskManager = new TaskManager ();
        Services.SceneStackManager = new SceneStackManager<TransitionData>(sceneRoot, Services.PrefabDB.Scenes);
    }

	

	

	
}
